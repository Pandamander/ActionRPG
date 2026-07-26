using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SandBall : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 2f;

    private Rigidbody2D _rigidBody;
    private bool _consumed;

    public void Initialize(Vector2 velocity)
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.velocity = velocity;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_consumed || !collision.CompareTag("Player"))
            return;

        if (!collision.TryGetComponent<IDamageable>(out var target))
            return;

        _consumed = true;
        target.Damage(damage, Utilities.DamageDirection(gameObject, collision.gameObject));
        Destroy(gameObject);
    }
}
