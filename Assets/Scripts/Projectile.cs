using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float timeToLive = 3f;
    [SerializeField] private float spinSpeed = 720f;
    [SerializeField] private float damageCooldown = 0.5f;

    private int _damage;
    private Rigidbody2D _rigidBody;
    private float _spinDirection;
    private float _damageCooldownTimer;

    public void Initialize(int damage, Vector2 velocity)
    {
        _damage = damage;
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.velocity = velocity;
        _spinDirection = velocity.x < 0f ? 1f : -1f;

        if (velocity.x < 0f)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        Destroy(gameObject, timeToLive);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * _spinDirection * Time.deltaTime);

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
