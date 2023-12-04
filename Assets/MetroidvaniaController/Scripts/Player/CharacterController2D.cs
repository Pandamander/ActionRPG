using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	private bool m_AirControl = true;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded { get; private set; }            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private float limitFallSpeed = 15f; // Limit fall speed

	private float postApexGravityMultiplier = 1f;

	//[SerializeField] private float m_DashForce = 25f;
	private bool canDash = false;
	private bool isDashing = false; //If player is dashing

	public float life = 10f; //Life of the player
	public bool invincible = false; //If player can die
	public bool canMove = true; //If player can move

	private Animator animator;

	[SerializeField] private float slopeCheckDistance;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D maxFriction;
    private CapsuleCollider2D capsuleCollider;
	private Vector2 capsuleColliderSize;
	private float slopeDownAngle;
	private Vector2 slopeNormalPerpendicular;
	private bool isOnSlope;
	private float slopeDownAngleOld;
	private float slopeSideAngle;
	private float horizMovement;
	private PlayerMovement playerMovement;

	[Header("Events")]
	[Space]

	public UnityEvent OnFallEvent;
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		capsuleCollider = GetComponent<CapsuleCollider2D>();
		capsuleColliderSize = capsuleCollider.size;
		playerMovement = GetComponent<PlayerMovement>();

		if (OnFallEvent == null)
			OnFallEvent = new UnityEvent();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
				if (!wasGrounded )
				{
					OnLandEvent.Invoke();
				}
		}

		if (!m_Grounded)
		{
			if (!playerMovement.isAttacking)
			{
                OnFallEvent.Invoke();
            }
        }

		SlopeCheck();
	}

	private void SlopeCheck()
	{
		Vector2 capsuleBottom = transform.position - new Vector3(0.0f, capsuleColliderSize.y / 2);
		SlopeCheckHorizontal(capsuleBottom);
		SlopeCheckVertical(capsuleBottom);
	}

	private void SlopeCheckHorizontal(Vector2 capsuleBottom)
	{
        RaycastHit2D slopeHitFont = Physics2D.Raycast(
			capsuleBottom,
			transform.right,
			slopeCheckDistance,
			m_WhatIsGround
		);

        RaycastHit2D slopeHitBack = Physics2D.Raycast(
			capsuleBottom,
			-transform.right,
			slopeCheckDistance,
			m_WhatIsGround
		);

		if (slopeHitFont)
		{
            slopeSideAngle = Vector2.Angle(slopeHitFont.normal, Vector2.up);

			if (slopeSideAngle < 88f)
			{
                isOnSlope = true;
            } else
			{
				isOnSlope = false;
			}
            Debug.DrawRay(slopeHitFont.point, slopeHitFont.normal, Color.cyan);
        } else if (slopeHitBack)
		{
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);

            if (slopeSideAngle < 88f)
            {
                isOnSlope = true;
            }
            else
            {
                isOnSlope = false;
            }
            Debug.DrawRay(slopeHitFont.point, slopeHitBack.normal, Color.green);
        } else
		{
			slopeSideAngle = 0.0f;
			isOnSlope = false;
		}
    }

    private void SlopeCheckVertical(Vector2 capsuleBottom)
    {
		RaycastHit2D hit = Physics2D.Raycast(
			capsuleBottom,
			Vector2.down,
			slopeCheckDistance,
			m_WhatIsGround
		);

		if (hit)
		{
			slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;

			slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

			if (slopeNormalPerpendicular.y < -0.5f)
			{
				isOnSlope = true;
			}

			slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.blue);

			if (isOnSlope && horizMovement == 0f)
			{
				 capsuleCollider.sharedMaterial = maxFriction;
			} else
			{
                capsuleCollider.sharedMaterial = noFriction;
			}
        }
    }

    public void Move(float move, bool jump, bool dash)
	{
		horizMovement = move;

		if (canMove) {
			if (dash && canDash)
			{
				//m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
				StartCoroutine(DashCooldown());
			}
			// If crouching, check to see if the character can stand up
			if (isDashing)
			{
				//m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
			}
			//only control the player if grounded or airControl is turned on
			else if ( (m_Grounded || m_AirControl) && !isOnSlope )
			{
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
			} else if (m_Grounded && isOnSlope) {

                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(
					-move * 10f * slopeNormalPerpendicular.x,
                    -move * 10f * slopeNormalPerpendicular.y
                );

				m_Rigidbody2D.velocity = targetVelocity;

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }

            if (!m_Grounded)
            {
                // post-apex gravity adjustment
                if (m_Rigidbody2D.velocity.y < 0f)
                {
                    Vector2 gravityAdjustment = Vector2.up * Physics2D.gravity * (postApexGravityMultiplier - 1);
                    m_Rigidbody2D.velocity += gravityAdjustment;
                }

                // Cap fall velocity
                if (m_Rigidbody2D.velocity.y < -limitFallSpeed)
                {
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);
                }
            }

            // If the player should jump...
            if (m_Grounded && !isOnSlope && jump)
			{
				// Add a vertical force to the player.
				if (!playerMovement.isAttacking)
				{
                    animator.SetBool("IsJumping", true);
                }

				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			}
		}
	}

	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(float damage, Vector3 position) 
	{
		if (!invincible)
		{
			//animator.SetBool("Hit", true);
			life -= damage;
			Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f ;
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.AddForce(damageDir * 10);
			if (life <= 0)
			{
				StartCoroutine(WaitToDead());
			}
			else
			{
				StartCoroutine(Stun(0.25f));
				StartCoroutine(MakeInvincible(1f));
			}
		}
	}

	IEnumerator DashCooldown()
	{
		//animator.SetBool("IsDashing", true);
		isDashing = true;
		canDash = false;
		yield return new WaitForSeconds(0.1f);
		isDashing = false;
		yield return new WaitForSeconds(0.5f);
		canDash = true;
	}

	IEnumerator Stun(float time) 
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}
	IEnumerator MakeInvincible(float time) 
	{
		invincible = true;
		yield return new WaitForSeconds(time);
		invincible = false;
	}
	IEnumerator WaitToMove(float time)
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	IEnumerator WaitToDead()
	{
		//animator.SetBool("IsDead", true);
		canMove = false;
		invincible = true;
		GetComponent<Attack>().enabled = false;
		yield return new WaitForSeconds(0.4f);
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
		yield return new WaitForSeconds(1.1f);
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}
}
