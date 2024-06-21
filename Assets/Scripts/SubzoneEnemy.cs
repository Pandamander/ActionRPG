using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class SubzoneEnemy : MonoBehaviour, IDamageable
{
    public float moveSpeed;
    public int attackDamage = 1;
    public int health;
    public SubzoneAudioManager audioManager;
    protected float patrolTime;
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected Animator _animator;
    protected bool _isDying = false;
    [SerializeField] protected CameraShake cameraShake;

    public virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {

    }

    protected void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

        moveSpeed *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.Damage(attackDamage, Utilities.DamageDirection(gameObject, collision.gameObject));
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
    public void Damage(int damage, float damageDirection)
    {
        health -= damage;
        if (health < 0) return;

        audioManager.PlayAttackHit();

        if (health == 0)
        {
            _isDying = true;
            _animator.SetBool("IsDead", true);
            rigidBody.velocity = Vector2.zero;

            if (TryGetComponent(out CapsuleCollider2D capsule))
            {
                capsule.enabled = false;
            } else if (TryGetComponent(out BoxCollider2D box))
            {
                box.enabled = false;
            }
            return;
        }
        StartCoroutine(TakeDamage());
    }

    public void EnemyDeathAnimationComplete()
    {
        Destroy(gameObject);
    }
}
