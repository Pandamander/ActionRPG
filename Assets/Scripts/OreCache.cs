using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class OreCache : MonoBehaviour, IMeleeDamageable
{
    [System.Serializable]
    public struct OreTypeWeights
    {
        [Range(0f, 100f)] public float largePercent;
        [Range(0f, 100f)] public float mediumPercent;
        [Range(0f, 100f)] public float smallPercent;
    }

    [Header("Stages")]
    [SerializeField] private Sprite[] stageSprites;
    [SerializeField] private float hitInvulnerabilityDuration = 0.35f;

    [Header("Ore Prefabs")]
    [SerializeField] private GameObject oreLargePrefab;
    [SerializeField] private GameObject oreMediumPrefab;
    [SerializeField] private GameObject oreSmallPrefab;

    [Header("Ore Drops")]
    [SerializeField] private Vector2Int stage1DropRange = new Vector2Int(3, 4);
    [SerializeField] private Vector2Int stage2DropRange = new Vector2Int(2, 3);
    [SerializeField] private Vector2Int stage3DropRange = new Vector2Int(1, 2);
    [SerializeField] private OreTypeWeights stage1Weights = new OreTypeWeights
    {
        largePercent = 50f,
        mediumPercent = 50f,
        smallPercent = 0f
    };
    [SerializeField] private OreTypeWeights stage2Weights = new OreTypeWeights
    {
        largePercent = 25f,
        mediumPercent = 50f,
        smallPercent = 25f
    };
    [SerializeField] private OreTypeWeights stage3Weights = new OreTypeWeights
    {
        largePercent = 0f,
        mediumPercent = 50f,
        smallPercent = 50f
    };

    [Header("Launch")]
    [SerializeField] private float minLaunchSpeed = 5f;
    [SerializeField] private float maxLaunchSpeed = 8f;
    [SerializeField] private float minLaunchAngle = 35f;
    [SerializeField] private float maxLaunchAngle = 75f;
    [SerializeField] private float spawnOffsetY = 0.25f;

    private SpriteRenderer spriteRenderer;
    private Collider2D cacheCollider;
    private int hitsTaken;
    private float invulnerableUntil;
    private bool isDestroyed;

    private const int MaxHits = 3;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cacheCollider = GetComponent<Collider2D>();
        ApplyStageSprite();
    }

    public void DamageFromMelee(float damageDirection)
    {
        if (isDestroyed || Time.time < invulnerableUntil)
        {
            return;
        }

        hitsTaken++;
        invulnerableUntil = Time.time + hitInvulnerabilityDuration;

        SpawnOreDrops(GetDropRangeForHit(hitsTaken), GetWeightsForHit(hitsTaken), damageDirection);

        if (hitsTaken >= MaxHits)
        {
            isDestroyed = true;
            Destroy(gameObject);
            return;
        }

        ApplyStageSprite();
    }

    private Vector2Int GetDropRangeForHit(int hitNumber)
    {
        switch (hitNumber)
        {
            case 1:
                return stage1DropRange;
            case 2:
                return stage2DropRange;
            default:
                return stage3DropRange;
        }
    }

    private OreTypeWeights GetWeightsForHit(int hitNumber)
    {
        switch (hitNumber)
        {
            case 1:
                return stage1Weights;
            case 2:
                return stage2Weights;
            default:
                return stage3Weights;
        }
    }

    private void ApplyStageSprite()
    {
        if (stageSprites == null || stageSprites.Length == 0)
        {
            return;
        }

        int spriteIndex = Mathf.Clamp(hitsTaken, 0, stageSprites.Length - 1);
        if (stageSprites[spriteIndex] != null)
        {
            spriteRenderer.sprite = stageSprites[spriteIndex];
        }
    }

    private void SpawnOreDrops(Vector2Int dropRange, OreTypeWeights weights, float damageDirection)
    {
        int minCount = Mathf.Min(dropRange.x, dropRange.y);
        int maxCount = Mathf.Max(dropRange.x, dropRange.y);
        int dropCount = Random.Range(minCount, maxCount + 1);
        float horizontalSign = damageDirection < 0f ? -1f : 1f;

        for (int i = 0; i < dropCount; i++)
        {
            GameObject prefab = ChooseOrePrefab(weights);
            if (prefab == null)
            {
                Debug.LogWarning($"OreCache '{name}' could not choose an ore prefab for this stage.", this);
                continue;
            }

            Vector3 spawnPosition = transform.position + new Vector3(0f, spawnOffsetY, 0f);
            GameObject oreObject = Instantiate(prefab, spawnPosition, Quaternion.identity);

            if (!oreObject.TryGetComponent<OrePickup>(out var orePickup))
            {
                Debug.LogWarning($"Ore prefab '{prefab.name}' is missing OrePickup.", this);
                continue;
            }

            float angle = Random.Range(minLaunchAngle, maxLaunchAngle) * Mathf.Deg2Rad;
            float speed = Random.Range(minLaunchSpeed, maxLaunchSpeed);
            Vector2 velocity = new Vector2(
                Mathf.Cos(angle) * speed * horizontalSign,
                Mathf.Sin(angle) * speed
            );

            orePickup.Launch(velocity, cacheCollider);
        }
    }

    private GameObject ChooseOrePrefab(OreTypeWeights weights)
    {
        float large = Mathf.Max(0f, weights.largePercent);
        float medium = Mathf.Max(0f, weights.mediumPercent);
        float small = Mathf.Max(0f, weights.smallPercent);
        float total = large + medium + small;

        if (total <= 0f)
        {
            return oreMediumPrefab != null ? oreMediumPrefab : oreLargePrefab != null ? oreLargePrefab : oreSmallPrefab;
        }

        float roll = Random.Range(0f, total);
        if (roll < large)
        {
            return oreLargePrefab;
        }

        if (roll < large + medium)
        {
            return oreMediumPrefab;
        }

        return oreSmallPrefab;
    }
}
