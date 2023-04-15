using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
	public Vector2 direction;
	public bool hasHit = false;
	public float speed;
	public float range;

	private bool isApplyingDamage = false;
	private Vector2 startingPosition = Vector2.zero;
	private Rigidbody2D rb;
	private bool attackShouldTranslate = false;

    private void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
		attackShouldTranslate = true;
	}

    // Start is called before the first frame update
    void Start()
	{
		startingPosition = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector2 positionDelta = new Vector2(transform.position.x, transform.position.y) - startingPosition;

		//Debug.Log(Vector2.SqrMagnitude(positionDelta));

		if (Vector2.SqrMagnitude(positionDelta) >= range)
		{
			attackShouldTranslate = false;
			StartCoroutine(Sheath());
		}

		if (attackShouldTranslate)
			rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
	}

	IEnumerator Sheath()
	{
		yield return new WaitForSeconds(0.3f);
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("MeleeWeapon: OnCollisionEnter2D: " + collision.gameObject.tag);

		if (!isApplyingDamage && collision.gameObject.tag == "Enemy")
		{
			isApplyingDamage = true;
			collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		Debug.Log("MeleeWeapon: OnTriggerEnter2D: " + collision.gameObject.tag);
	}
}