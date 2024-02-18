using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour, IDamageable
{
	public float dmgValue = 4f;
	public int invulnerableDuration = 10;
	public MeleeController meleeWeaponController;
	public GameObject rangedWeaponController;

	public Transform attackCheck;
	private Rigidbody2D rigidBody;
	public Animator animator;
	public bool canMeleeAttack = true;
	public bool isTimeToCheck = false;
	public GameObject cam;
	public SubzoneAudioManager audioManager;
	public Vector2 knockbackForce = Vector2.zero;
	public bool isAttacking
	{
		get
		{
			return !canMeleeAttack;
		}
	}
	public bool isDamaged { get; private set; }

    [SerializeField] private SubzoneHUD subzoneHUD;

	private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private CharacterController2D controller;
	private bool shouldCheckGroundedForKnockback = false;
    private bool playerWasKnockedBack = false;
    private bool dead = false;
    private const int PLAYER_COLLISION_LAYER = 1;
    private const int ENEMY_COLLISION_LAYER = 9;

    private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        controller = gameObject.GetComponent<CharacterController2D>();

        // TODO: Elliott remove this
        PlayerStats.Initialize();
	}

    // Update is called once per frame
    void Update()
    {
		// Update melee attack direction
		meleeWeaponController.playerDirection = transform.localScale.x > 0 ?
			MeleeController.PlayerDirection.Right : MeleeController.PlayerDirection.Left;
		meleeWeaponController.isCrouching = playerMovement.isCrouching;

		// Handle melee attack
		if (meleeWeaponController.HasWeapon && playerMovement.canMove && Input.GetButtonDown("Fire1") && canMeleeAttack)
		{
			if (playerMovement.grounded)
			{
                playerMovement.StopForAttack();
            } else
			{
				playerMovement.StopAirControlForJumpAttack();
			}
            canMeleeAttack = false;
			animator.SetBool("IsAttacking", true);

            meleeWeaponController.Attack();

			StartCoroutine(MeleeAttackCooldown());
		}

		// Handle knockback landing
		if (shouldCheckGroundedForKnockback && animator.GetBool("IsHit"))
		{
			CheckGroundedForKnockback();
        }
	}

	IEnumerator MeleeAttackCooldown()
	{
		yield return new WaitForSeconds(0.25f);
		animator.SetBool("IsAttacking", false);
        canMeleeAttack = true;
		playerMovement.AllowMovement();
    }

    private IEnumerator Invulnerability(int duration)
    {
        Color color = Color.clear;
		int durationCounter = 0;
        while (durationCounter <= duration)
        {
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.1f);
            color = (color == Color.clear) ? Color.white : Color.clear;
			durationCounter++;
        }
        spriteRenderer.color = Color.white;
        Physics2D.IgnoreLayerCollision(PLAYER_COLLISION_LAYER, ENEMY_COLLISION_LAYER, false);
    }

	private void CheckGroundedForKnockback()
	{
        if (!playerMovement.grounded)
        {
            playerWasKnockedBack = true;
        }
        else
        {
            if (playerWasKnockedBack)
            {
                shouldCheckGroundedForKnockback = false;
                playerWasKnockedBack = false;
                StartCoroutine(ResumeControlAfterKnockback());
            }
        }
    }

    private IEnumerator ResumeControlAfterKnockback()
	{
        if (PlayerStats.Health <= 0f)
        {
            yield return StartCoroutine(Die());
        } else
		{
            animator.SetBool("IsHit", false);
            isDamaged = false;
            playerMovement.AllowMovement();
            canMeleeAttack = true;
            yield return StartCoroutine(Invulnerability(invulnerableDuration));
        }
    }

	// IDamageable
	public void Damage(int damage)
	{
		if (dead) { return; }

		if (!isDamaged)
		{
            isDamaged = true;
			Physics2D.IgnoreLayerCollision(PLAYER_COLLISION_LAYER, ENEMY_COLLISION_LAYER, true);
            subzoneHUD.ReducePlayerHealthMeter(damage);
            audioManager.PlayDamage();
            PlayerStats.ApplyDamage(damage);

			playerMovement.StopForKnockback();
			canMeleeAttack = false;
            animator.SetBool("IsHit", true);
            float knockbackDirection = transform.localScale.x > 0f ? -1f : 1f;
			rigidBody.AddForce(new Vector2(knockbackForce.x * knockbackDirection, knockbackForce.y), ForceMode2D.Impulse);
			shouldCheckGroundedForKnockback = true;
        }
	}

    private IEnumerator Die()
	{
        audioManager.PlayGameOver();
        dead = true;
        animator.SetBool("IsDead", true);
		yield return new WaitForSeconds(4);
        GameManager.sharedInstance.ShowGameOver(SceneManager.GetActiveScene().name);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "GladiusItem")
        {
            MeleeWeaponPickup weaponPickup = collision.gameObject.GetComponentInChildren<MeleeWeaponPickup>();
            if (weaponPickup != null)
            {
                meleeWeaponController.SetMeleeWeapon(weaponPickup.weapon);
            }
        }
    }
}
