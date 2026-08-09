using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DenariusPickup : MonoBehaviour
{
    [SerializeField] private int denariusAmount = 1;
    [SerializeField] private GameObject pickupFxPrefab;
    [SerializeField] private FloatingPickupText pickupTextPrefab;
    [SerializeField] private float popupHeadOffset = 1.2f;

    private bool collected;

    public void Launch(Vector2 velocity, Collider2D ignoreCollider)
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = velocity;

        Collider2D denariusCollider = GetComponent<Collider2D>();
        if (ignoreCollider != null && denariusCollider != null)
        {
            Physics2D.IgnoreCollision(denariusCollider, ignoreCollider);
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
        PlayerStats.AddDenarius(denariusAmount);
        SpawnPickupFx();
        SpawnPickupPopup(other.transform);
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

    private void SpawnPickupPopup(Transform player)
    {
        if (pickupTextPrefab == null || player == null)
        {
            return;
        }

        Vector3 spawnPosition = player.position + Vector3.up * popupHeadOffset;
        FloatingPickupText.Show(pickupTextPrefab, spawnPosition, denariusAmount);
    }
}
