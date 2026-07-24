using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
public class CrabSpawnZone : MonoBehaviour
{
    [SerializeField] private SubzoneCrab crabPrefab;
    [SerializeField] private SandEffect sandEffectPrefab;
    [SerializeField] private GameObject sandBurrowEffectPrefab;
    [FormerlySerializedAs("sandEffectYOffset")]
    [SerializeField] private float sandBurstEffectYOffset = 0.5f;
    [SerializeField] private float sandBurrowEffectYOffset = 0.5f;
    [SerializeField] private BoxCollider2D boundsCollider;
    [SerializeField] private Transform surfacePoint;
    [SerializeField] private Transform undergroundPoint;
    [SerializeField] private SpriteMask groundMask;
    [SerializeField] private int maxActiveCrabs = 2;
    [SerializeField] private float minSpawnSpacing = 2f;
    [SerializeField] private float initialSpawnDelay = 3f;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private float spawnStagger = 1.5f;
    [SerializeField] private bool loopSpawns = true;
    [SerializeField] private SubzoneAudioManager audioManager;

    private readonly List<SubzoneCrab> _activeCrabs = new();
    private readonly Queue<float> _spawnTimes = new();
    private bool _shuttingDown;

    private void Awake()
    {
        if (boundsCollider == null)
            boundsCollider = GetComponent<BoxCollider2D>();

        boundsCollider.isTrigger = true;

        if (audioManager == null)
            audioManager = FindObjectOfType<SubzoneAudioManager>();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (boundsCollider == null)
            boundsCollider = GetComponent<BoxCollider2D>();

    }
#endif

    private void OnDestroy()
    {
        _shuttingDown = true;
        _spawnTimes.Clear();
    }

    private void Start()
    {
        ScheduleSpawns(maxActiveCrabs, initialSpawnDelay);
    }

    private void Update()
    {
        _activeCrabs.RemoveAll(crab => crab == null);

        while (_spawnTimes.Count > 0 && Time.time >= _spawnTimes.Peek())
        {
            _spawnTimes.Dequeue();

            if (_activeCrabs.Count < maxActiveCrabs)
                SpawnCrab();
        }
    }

    public CrabEmergePlacement RequestEmergePlacement(SubzoneCrab requestingCrab)
    {
        Bounds bounds = boundsCollider.bounds;
        float minX = bounds.min.x;
        float maxX = bounds.max.x;
        float width = maxX - minX;
        float thirdWidth = width / 3f;

        const int maxAttempts = 16;
        float bestSpawnX = 0f;
        float bestMoveDirection = 1f;
        float bestClearance = -1f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            int zone = Random.Range(0, 3);
            float spawnX;
            float moveDirection;

            switch (zone)
            {
                case 0:
                    spawnX = Random.Range(minX, minX + thirdWidth);
                    moveDirection = 1f;
                    break;
                case 1:
                    spawnX = Random.Range(minX + thirdWidth, maxX - thirdWidth);
                    moveDirection = Random.value < 0.5f ? -1f : 1f;
                    break;
                default:
                    spawnX = Random.Range(maxX - thirdWidth, maxX);
                    moveDirection = -1f;
                    break;
            }

            float clearance = GetSpawnClearance(spawnX, requestingCrab);
            if (clearance >= minSpawnSpacing)
            {
                return BuildPlacement(spawnX, moveDirection, minX, maxX);
            }

            if (clearance > bestClearance)
            {
                bestClearance = clearance;
                bestSpawnX = spawnX;
                bestMoveDirection = moveDirection;
            }
        }

        return BuildPlacement(bestSpawnX, bestMoveDirection, minX, maxX);
    }

    private float GetSpawnClearance(float spawnX, SubzoneCrab requestingCrab)
    {
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < _activeCrabs.Count; i++)
        {
            SubzoneCrab crab = _activeCrabs[i];
            if (crab == null || crab == requestingCrab)
                continue;

            if (crab.State != SubzoneCrab.CrabState.Telegraphing &&
                crab.State != SubzoneCrab.CrabState.Appearing)
                continue;

            float distance = Mathf.Abs(crab.transform.position.x - spawnX);
            if (distance < nearestDistance)
                nearestDistance = distance;
        }

        return nearestDistance;
    }

    private CrabEmergePlacement BuildPlacement(float spawnX, float moveDirection, float minX, float maxX)
    {
        return new CrabEmergePlacement
        {
            SpawnX = spawnX,
            MoveDirection = moveDirection,
            MinX = minX,
            MaxX = maxX,
            SurfaceY = surfacePoint.position.y,
            UndergroundY = undergroundPoint.position.y
        };
    }

    public void SpawnSandEffect(float emergeX)
    {
        if (sandEffectPrefab == null || surfacePoint == null)
            return;

        Vector3 position = new Vector3(emergeX, surfacePoint.position.y + sandBurstEffectYOffset, transform.position.z);
        Instantiate(sandEffectPrefab, position, Quaternion.identity);
    }

    public GameObject SpawnSandBurrowEffect(float emergeX)
    {
        if (sandBurrowEffectPrefab == null || surfacePoint == null)
            return null;

        Vector3 position = new Vector3(emergeX, surfacePoint.position.y + sandBurrowEffectYOffset, transform.position.z);
        return Instantiate(sandBurrowEffectPrefab, position, Quaternion.identity);
    }

    public void NotifyCrabRemoved(SubzoneCrab crab)
    {
        if (_shuttingDown)
            return;

        if (!_activeCrabs.Remove(crab))
            return;

        if (loopSpawns)
            ScheduleSpawns(1, respawnDelay);
    }

    private void ScheduleSpawns(int count, float baseDelay)
    {
        for (int i = 0; i < count; i++)
        {
            int queueIndex = _spawnTimes.Count;
            _spawnTimes.Enqueue(Time.time + baseDelay + queueIndex * spawnStagger);
        }
    }

    private void SpawnCrab()
    {
        if (crabPrefab == null)
        {
            Debug.LogWarning($"{nameof(CrabSpawnZone)} on {name} is missing a crab prefab.", this);
            return;
        }

        if (surfacePoint == null || undergroundPoint == null)
        {
            Debug.LogWarning($"{nameof(CrabSpawnZone)} on {name} requires surface and underground points.", this);
            return;
        }

        CrabEmergePlacement placement = RequestEmergePlacement(null);
        SubzoneCrab crab = Instantiate(crabPrefab);
        crab.Initialize(this, placement, audioManager);
        _activeCrabs.Add(crab);
    }

    private void OnDrawGizmosSelected()
    {
        BoxCollider2D collider = boundsCollider != null ? boundsCollider : GetComponent<BoxCollider2D>();
        if (collider == null)
            return;

        Gizmos.color = new Color(0.9f, 0.7f, 0.2f, 0.35f);
        Bounds bounds = collider.bounds;
        Gizmos.DrawCube(bounds.center, bounds.size);

        if (surfacePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(surfacePoint.position, 0.15f);
        }

        if (undergroundPoint != null)
        {
            Gizmos.color = new Color(0.6f, 0.4f, 0.1f);
            Gizmos.DrawSphere(undergroundPoint.position, 0.15f);
        }
    }
}
