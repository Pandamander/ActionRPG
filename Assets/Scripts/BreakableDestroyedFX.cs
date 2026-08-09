using UnityEngine;

public class BreakableDestroyedFX : MonoBehaviour
{
    [Header("Debris")]
    [SerializeField] private BreakableDebris debrisPrefab;
    [SerializeField] private int debrisCountMin = 4;
    [SerializeField] private int debrisCountMax = 4;
    [SerializeField] private float spawnOffsetY = 0f;
    [SerializeField] private float spawnSpreadX = 0.1f;

    [Header("Debris launch properties")]
    [SerializeField] private float minLaunchSpeed = 3f;
    [SerializeField] private float maxLaunchSpeed = 6f;
    [SerializeField] private float minLaunchAngle = 35f;
    [SerializeField] private float maxLaunchAngle = 145f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float minAngularVelocity = -360f;
    [SerializeField] private float maxAngularVelocity = 360f;

    [Header("Debris scale")]
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 1f;

    private void Start()
    {
        SpawnDebris();
        Destroy(gameObject);
    }

    private void SpawnDebris()
    {
        if (debrisPrefab == null)
        {
            Debug.LogWarning($"BreakableDestroyedFX '{name}' is missing a debris prefab.", this);
            return;
        }

        int minCount = Mathf.Max(0, Mathf.Min(debrisCountMin, debrisCountMax));
        int maxCount = Mathf.Max(debrisCountMin, debrisCountMax);
        int count = Random.Range(minCount, maxCount + 1);

        Vector3 basePosition = transform.position + new Vector3(0f, spawnOffsetY, 0f);

        for (int i = 0; i < count; i++)
        {
            float offsetX = spawnSpreadX > 0f ? Random.Range(-spawnSpreadX, spawnSpreadX) : 0f;
            Vector3 spawnPosition = basePosition + new Vector3(offsetX, 0f, 0f);

            BreakableDebris debris = Instantiate(debrisPrefab, spawnPosition, Quaternion.identity);

            float scale = Random.Range(minScale, maxScale);
            debris.transform.localScale = Vector3.one * scale;

            float angle = Random.Range(minLaunchAngle, maxLaunchAngle) * Mathf.Deg2Rad;
            float speed = Random.Range(minLaunchSpeed, maxLaunchSpeed);
            Vector2 velocity = new Vector2(
                Mathf.Cos(angle) * speed,
                Mathf.Sin(angle) * speed
            );

            float angularVelocity = Random.Range(minAngularVelocity, maxAngularVelocity);
            debris.Initialize(velocity, gravityScale, angularVelocity);
        }
    }
}
