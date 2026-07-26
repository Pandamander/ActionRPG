using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class OrePickup : MonoBehaviour
{
    [SerializeField] private int oreAmount = 2;
    [SerializeField] private GameObject pickupFxPrefab;

    private bool collected;

    public void Launch(Vector2 velocity, Collider2D ignoreCollider)
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = velocity;

        Collider2D oreCollider = GetComponent<Collider2D>();
        if (ignoreCollider != null && oreCollider != null)
        {
            Physics2D.IgnoreCollision(oreCollider, ignoreCollider);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryCollect(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryCollect(other.gameObject);
    }

    private void TryCollect(GameObject other)
    {
        if (collected || !other.CompareTag("Player"))
        {
            return;
        }

        collected = true;
        PlayerStats.AddOre(oreAmount);
        SpawnPickupFx();
        Destroy(gameObject);
    }

    private void SpawnPickupFx()
    {
        if (pickupFxPrefab == null)
        {
            return;
        }

        Instantiate(pickupFxPrefab, transform.position, Quaternion.identity);
    }
}
