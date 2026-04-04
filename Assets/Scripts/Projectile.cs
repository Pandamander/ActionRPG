using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float timeToLive = 3f;
    [SerializeField] private float spinSpeed = 720f;
    [SerializeField] private bool alignToVelocity = false;
    [SerializeField] private float damageCooldown = 0.5f;

    private int _damage;
    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;
    private float _spinDirection;
    private float _damageCooldownTimer;

    public void Initialize(int damage, Vector2 velocity)
    {
        _damage = damage;
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody.velocity = velocity;
        _spinDirection = velocity.x < 0f ? 1f : -1f;

        if (!alignToVelocity && velocity.x < 0f)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        Destroy(gameObject, timeToLive);
    }

    private void Update()
    {
        if (alignToVelocity)
        {
            Vector2 vel = _rigidBody.velocity;
            bool goingLeft = vel.x < 0f;
            float angle = Mathf.Atan2(vel.y, Mathf.Abs(vel.x)) * Mathf.Rad2Deg;
            if (goingLeft) angle = -angle;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            _spriteRenderer.flipX = goingLeft;
        }
        else
        {
            transform.Rotate(0f, 0f, spinSpeed * _spinDirection * Time.deltaTime);
        }

        if (_damageCooldownTimer > 0f)
            _damageCooldownTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_damageCooldownTimer > 0f) return;

        if (collision.TryGetComponent<IDamageable>(out var target))
        {
            target.Damage(_damage, Utilities.DamageDirection(gameObject, collision.gameObject));
            _damageCooldownTimer = damageCooldown;
        }
    }
}
