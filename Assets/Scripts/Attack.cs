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

	[SerializeField] private SubzoneHUD subzoneHUD;

	private SpriteRenderer spriteRenderer;
	//private bool dead = false;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		// TODO: Elliott remove this
		PlayerStats.Initialize();
	}

    // Update is called once per frame
    void Update()
    {
		meleeWeaponController.playerDirection = transform.localScale.x > 0 ?
			MeleeController.PlayerDirection.Right : MeleeController.PlayerDirection.Left;

		if (Input.GetButtonDown("Fire1") && canMeleeAttack)
		{
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
	}

    private IEnumerator TakeDamage()
	{
		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
		yield return StartCoroutine(Invulnerability(invulnerableDuration));
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
		animator.SetTrigger("IsDamaged");
        if (PlayerStats.Health <= 0f)
        {
			Die();
            GetComponent<CapsuleCollider2D>().enabled = false;
        } else
		{
			float knockbackForce = transform.localScale.x > 0f ? -2000f : 2000f;
            rigidBody.AddForce(new Vector2(knockbackForce, 0f));
            StartCoroutine(TakeDamage());
        }
    }

    private void Die()
	{
		//dead = true;
		spriteRenderer.color = Color.red;
		CharacterController2D controller = gameObject.GetComponent<CharacterController2D>();
		controller.canMove = false;
		PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();
		playerMovement.Stop();
	}
}
