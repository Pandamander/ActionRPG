using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubzoneEnemy : MonoBehaviour
{
    public float moveSpeed;
    public float patrolFlipTime;
    public float health;
    public bool isVertical;
    public SubzoneAudioManager audioManager;
    private float patrolTime;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (health <= 0f)
        {
            rigidBody.velocity = new Vector2(
                0f,
                -200 * Time.fixedDeltaTime
            );
            transform.Rotate(0f, 0f, 2 * 360 * Time.deltaTime);
            return;
        }

        patrolTime += Time.deltaTime;
        if (patrolTime >= patrolFlipTime)
        {
            Flip();
            patrolTime = 0f;
        }

        if (isVertical)
        {
            rigidBody.velocity = new Vector2(
                rigidBody.velocity.x,
                -moveSpeed * Time.fixedDeltaTime
            );
        } else
        {
            rigidBody.velocity = new Vector2(
                -moveSpeed * Time.fixedDeltaTime,
                rigidBody.velocity.y
            );
        }
    }

    private void Flip()
    {
        if (!isVertical)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }

        moveSpeed *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SubzoneHeroProjectile"))
        {
            audioManager.PlayDamage();
            health -= PlayerStats.Attack;

            StartCoroutine(TakeDamage());
        }
    }

    private IEnumerator TakeDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
