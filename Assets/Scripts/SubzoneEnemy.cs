using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SubzoneEnemy : MonoBehaviour, IDamageable
{
    public float moveSpeed;
    public float patrolFlipTime;
    public float attackDamage = 1f;
    public float health;
    public bool isVertical;
    public SubzoneAudioManager audioManager;
    protected float patrolTime;
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected CameraShake cameraShake;

    public virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
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
    }

    protected void Flip()
    {
        if (!isVertical)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }

        moveSpeed *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.Damage(attackDamage);
            }
        }
    }

    private IEnumerator TakeDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    // IDamageable
    public void Damage(float damage)
    {
        cameraShake.ShakeCamera(0.15f, 1.5f);
        health -= damage;
        if (health <= 0f)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        StartCoroutine(TakeDamage());
    }
}
