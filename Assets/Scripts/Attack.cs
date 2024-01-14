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
	public float knockbackLoseControlDuration = 1f;
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
    //private bool dead = false;

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
		meleeWeaponController.playerDirection = transform.localScale.x > 0 ?
			MeleeController.PlayerDirection.Right : MeleeController.PlayerDirection.Left;

		if (playerMovement.canMove && Input.GetButtonDown("Fire1") && canMeleeAttack)
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
		Physics2D.IgnoreLayerCollision(1, 9, true);
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
        Physics2D.IgnoreLayerCollision(1, 9, false);
    }

	private IEnumerator ResumeControlAfterKnockback()
	{
		yield return new WaitForSeconds(knockbackLoseControlDuration);
        animator.SetBool("IsHit", false);
		isDamaged = false;
        playerMovement.AllowMovement();
		canMeleeAttack = true;
        yield return StartCoroutine(Invulnerability(invulnerableDuration));
    }

    private IEnumerator GameOver()
	{
		audioManager.PlayGameOver();
		yield return new WaitForSeconds(3.0f);
		PlayerStats.Reset();
		OverworldSubzoneContainer.Reset();
		SceneManager.LoadScene("GameOver");
	}

	// IDamageable
	public void Damage(float damage)
	{
		subzoneHUD.ReducePlayerHealthMeter((int)damage);
		audioManager.PlayDamage();
		PlayerStats.ApplyDamage(damage);
		animator.SetBool("IsHit", true);
		if (PlayerStats.Health <= 0f)
		{
			Die();
			GetComponent<CapsuleCollider2D>().enabled = false;
		}
		else if (!isDamaged)
		{
			isDamaged = true;
			playerMovement.StopForKnockback();
			canMeleeAttack = false;
            animator.SetBool("IsHit", true);
            float knockbackDirection = transform.localScale.x > 0f ? -1f : 1f;
			rigidBody.AddForce(new Vector2(knockbackForce.x * knockbackDirection, knockbackForce.y), ForceMode2D.Impulse);
			StartCoroutine(ResumeControlAfterKnockback());
		}
	}

    private void Die()
	{
		//dead = true;
		spriteRenderer.color = Color.red;
		controller = gameObject.GetComponent<CharacterController2D>();
		controller.canMove = false;
		playerMovement.Stop();
	}
}
