using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour
{
	public float dmgValue = 4;
	public GameObject meleeWeapon1;
	public GameObject throwableObject1;
	public GameObject throwableObject2;
	public GameObject throwableObject3;
	public GameObject throwableObject4;
	public GameObject throwableObject5;
	public Transform attackCheck;
	private Rigidbody2D rigidBody;
	public Animator animator;
	public bool canAttack = true;
	public bool isTimeToCheck = false;
	public GameObject cam;
	public SubzoneAudioManager audioManager;

	private SpriteRenderer spriteRenderer;
	private bool dead = false;

    [SerializeField] private GameObject meleeCollider;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		meleeCollider.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Fire1") && canAttack)
		{
			canAttack = false;
			animator.SetBool("IsAttacking", true);

			Vector2 direction = new Vector2(transform.localScale.x, 0);

			GameObject weaponPrefab;
			bool isMelee = false;
			switch (PlayerStats.Attack)
            {
				case 1:
					weaponPrefab = meleeWeapon1;
					isMelee = true;
					break;
				case 2:
					weaponPrefab = throwableObject2;
					break;
				case 3:
					weaponPrefab = throwableObject3;
					break;
				case 4:
					weaponPrefab = throwableObject4;
					break;
				case 5:
					weaponPrefab = throwableObject5;
					break;
				default:
					weaponPrefab = meleeWeapon1;
					isMelee = true;
					break;
			}

			GameObject weapon;
			if (isMelee)
            {
				meleeCollider.SetActive(true);
			} else
            {
				weapon = Instantiate(
					weaponPrefab,
					transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f),
					Quaternion.Euler(0f, 0f, direction.x > 0 ? 0f : 180f)
				) as GameObject;
				weapon.GetComponent<ThrowableWeapon>().direction = direction;
				weapon.name = "ThrowableWeapon";
			}

			audioManager.PlayAttack();

			StartCoroutine(AttackCooldown());
		}
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.25f);
		animator.SetBool("IsAttacking", false);
		canAttack = true;
		meleeCollider.SetActive(false);
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("SubzoneEnemy"))
        {
			if (dead) { return; }

			audioManager.PlayDamage();
			PlayerStats.ApplyDamage(1f);

			if (PlayerStats.Health <= 0f)
            {
				Die();
				StartCoroutine(GameOver());
            }
			else
            {
				rigidBody.AddForce(new Vector2(-2000f, 0f));
				StartCoroutine(TakeDamage());
			}			
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.CompareTag("Boss"))
		{
			if (dead) { return; }

			audioManager.PlayDamage();
			PlayerStats.ApplyDamage(1f);

			if (PlayerStats.Health <= 0f)
			{
				Die();
				StartCoroutine(GameOver());
			}
			else
			{
				rigidBody.AddForce(new Vector2(-2000f, 0f));
				StartCoroutine(TakeDamage());
			}
		}
	}

    private IEnumerator TakeDamage()
	{
		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(0.1f);
		spriteRenderer.color = Color.white;
	}

	private IEnumerator GameOver()
	{
		audioManager.PlayGameOver();
		yield return new WaitForSeconds(3.0f);
		PlayerStats.Reset();
		OverworldSubzoneContainer.Reset();
		SceneManager.LoadScene("GameOver");
	}

	private void Die()
	{
		dead = true;
		spriteRenderer.color = Color.red;
		CharacterController2D controller = gameObject.GetComponent<CharacterController2D>();
		controller.canMove = false;
		PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();
		playerMovement.Stop();
	}
}
