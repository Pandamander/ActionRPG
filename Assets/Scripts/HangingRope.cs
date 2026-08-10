using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HangingRope : MonoBehaviour
{
    [Header("Shape (local space)")]
    [SerializeField] private Vector2 localStart = Vector2.zero;
    [SerializeField] private Vector2 localEnd = new Vector2(0f, -2f);

    [Header("Pixels")]
    [SerializeField] private float pixelsPerUnit = 16f;
    [SerializeField] private bool useGradient;
    [SerializeField] private Color ropeColor = new Color(0.55f, 0.4f, 0.25f, 1f);
    [SerializeField] private Gradient ropeGradient;
    [SerializeField] private string sortingLayerName = "Default";
    [SerializeField] private int sortingOrder = 4;

    [Header("Sway")]
    [Tooltip("Maximum swing angle in degrees. 0 = no sway.")]
    [SerializeField] private float swayAmount = 12f;
    [SerializeField] private float period = 3f;
    [Tooltip("Offset into the sway cycle as a fraction 0–1 (0 and 1 are the same point in the cycle).")]
    [SerializeField] [Range(0f, 1f)] private float phase;

    [Header("Attachment (optional)")]
    [SerializeField] private GameObject attachmentPrefab;
    [SerializeField] private Vector2 attachmentLocalOffset;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;
    private Transform attachment;
    private Rigidbody2D attachmentBody;
    private Vector2 currentTipLocal;
    private readonly List<Vector2Int> lineCells = new List<Vector2Int>(64);
    private readonly List<Vector3> vertices = new List<Vector3>(256);
    private readonly List<int> triangles = new List<int>(384);
    private readonly List<Color> colors = new List<Color>(256);
    private Material runtimeMaterial;

    private float PixelSize => 1f / Mathf.Max(1f, pixelsPerUnit);

    private void Reset()
    {
        // Sensible multi-stop default when the component is first added
        ropeGradient = CreateDefaultGradient();
    }

    private static Gradient CreateDefaultGradient()
    {
        var gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(0.65f, 0.5f, 0.32f), 0f),
                new GradientColorKey(new Color(0.55f, 0.4f, 0.25f), 0.5f),
                new GradientColorKey(new Color(0.35f, 0.25f, 0.15f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
        return gradient;
    }

    private void Awake()
    {
        if (ropeGradient == null)
        {
            ropeGradient = CreateDefaultGradient();
        }

        CacheComponents();
        EnsureMeshAndMaterial();
        TrySpawnAttachment();
        ApplyRope(0f, forceImmediate: true);
    }

    private void FixedUpdate()
    {
        ApplyRope(Time.time, forceImmediate: false);
    }

    private void OnDestroy()
    {
        if (mesh != null)
        {
            if (Application.isPlaying)
            {
                Destroy(mesh);
            }
            else
            {
                DestroyImmediate(mesh);
            }
        }

        if (runtimeMaterial != null)
        {
            if (Application.isPlaying)
            {
                Destroy(runtimeMaterial);
            }
            else
            {
                DestroyImmediate(runtimeMaterial);
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        period = Mathf.Max(0.01f, period);
        swayAmount = Mathf.Max(0f, swayAmount);
        phase = Mathf.Clamp01(phase);
        pixelsPerUnit = Mathf.Max(1f, pixelsPerUnit);

        if (Application.isPlaying)
        {
            return;
        }

        // Preview rope in edit mode at rest
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null)
            {
                return;
            }

            CacheComponents();
            EnsureMeshAndMaterial();
            ApplyRope(0f, forceImmediate: true);
        };
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 start = transform.TransformPoint(localStart);
        Vector3 end = transform.TransformPoint(localEnd);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, 0.05f);
        Gizmos.DrawWireSphere(end, 0.05f);
    }
#endif

    private void CacheComponents()
    {
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
    }

    private void EnsureMeshAndMaterial()
    {
        if (mesh == null)
        {
            mesh = new Mesh { name = "HangingRopeMesh" };
            mesh.MarkDynamic();
        }

        if (meshFilter != null)
        {
            meshFilter.sharedMesh = mesh;
        }

        if (meshRenderer == null)
        {
            return;
        }

        if (runtimeMaterial == null)
        {
            Shader shader = Shader.Find("Sprites/Default");
            runtimeMaterial = new Material(shader != null ? shader : Shader.Find("Unlit/Color"));
            runtimeMaterial.name = "HangingRopeMaterial";
        }

        meshRenderer.sharedMaterial = runtimeMaterial;
        meshRenderer.sortingLayerName = sortingLayerName;
        meshRenderer.sortingOrder = sortingOrder;
    }

    private void TrySpawnAttachment()
    {
        if (attachmentPrefab == null || attachment != null)
        {
            return;
        }

        GameObject instance = Instantiate(attachmentPrefab, transform);
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = Vector3.one;
        attachment = instance.transform;
        attachmentBody = instance.GetComponent<Rigidbody2D>();

        if (attachmentBody != null)
        {
            attachmentBody.bodyType = RigidbodyType2D.Kinematic;
            attachmentBody.gravityScale = 0f;
            attachmentBody.interpolation = RigidbodyInterpolation2D.Interpolate;
            attachmentBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            attachmentBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void ApplyRope(float time, bool forceImmediate)
    {
        float angleRad = 0f;
        if (swayAmount > 0f && period > 0f)
        {
            float omega = (Mathf.PI * 2f) / period;
            float phaseRadians = phase * Mathf.PI * 2f;
            angleRad = swayAmount * Mathf.Deg2Rad * Mathf.Sin(omega * time + phaseRadians);
        }

        Vector2 pivot = localStart;
        Vector2 endOffset = localEnd - localStart;
        float sin = Mathf.Sin(angleRad);
        float cos = Mathf.Cos(angleRad);
        Vector2 swayedEnd = pivot + new Vector2(
            endOffset.x * cos - endOffset.y * sin,
            endOffset.x * sin + endOffset.y * cos
        );

        currentTipLocal = swayedEnd;

        Vector2 startWorld = transform.TransformPoint(localStart);
        Vector2 endWorld = transform.TransformPoint(swayedEnd);

        lineCells.Clear();
        RasterizeBresenham(WorldToGrid(startWorld), WorldToGrid(endWorld), lineCells);
        RebuildMesh(lineCells);
        UpdateAttachment(forceImmediate);
    }

    private Vector2Int WorldToGrid(Vector2 world)
    {
        float size = PixelSize;
        return new Vector2Int(
            Mathf.FloorToInt(world.x / size),
            Mathf.FloorToInt(world.y / size)
        );
    }

    private static void RasterizeBresenham(Vector2Int from, Vector2Int to, List<Vector2Int> cells)
    {
        int x0 = from.x;
        int y0 = from.y;
        int x1 = to.x;
        int y1 = to.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            cells.Add(new Vector2Int(x0, y0));
            if (x0 == x1 && y0 == y1)
            {
                break;
            }

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    private void RebuildMesh(List<Vector2Int> cells)
    {
        if (mesh == null)
        {
            return;
        }

        vertices.Clear();
        triangles.Clear();
        colors.Clear();

        float size = PixelSize;
        int vertBase = 0;

        for (int i = 0; i < cells.Count; i++)
        {
            Vector2Int cell = cells[i];
            // World corner of cell, then to local
            Vector3 worldMin = new Vector3(cell.x * size, cell.y * size, 0f);
            Vector3 worldMax = new Vector3((cell.x + 1) * size, (cell.y + 1) * size, 0f);

            Vector3 bl = transform.InverseTransformPoint(worldMin);
            Vector3 br = transform.InverseTransformPoint(new Vector3(worldMax.x, worldMin.y, 0f));
            Vector3 tr = transform.InverseTransformPoint(worldMax);
            Vector3 tl = transform.InverseTransformPoint(new Vector3(worldMin.x, worldMax.y, 0f));

            vertices.Add(bl);
            vertices.Add(br);
            vertices.Add(tr);
            vertices.Add(tl);

            Color cellColor = GetColorAlongRope(i, cells.Count);
            colors.Add(cellColor);
            colors.Add(cellColor);
            colors.Add(cellColor);
            colors.Add(cellColor);

            triangles.Add(vertBase + 0);
            triangles.Add(vertBase + 1);
            triangles.Add(vertBase + 2);
            triangles.Add(vertBase + 0);
            triangles.Add(vertBase + 2);
            triangles.Add(vertBase + 3);
            vertBase += 4;
        }

        mesh.Clear();
        if (vertices.Count == 0)
        {
            return;
        }

        mesh.SetVertices(vertices);
        mesh.SetColors(colors);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateBounds();
    }

    private Color GetColorAlongRope(int cellIndex, int cellCount)
    {
        if (!useGradient || ropeGradient == null)
        {
            return ropeColor;
        }

        float t = cellCount <= 1 ? 0f : cellIndex / (float)(cellCount - 1);
        return ropeGradient.Evaluate(t);
    }

    private void UpdateAttachment(bool forceImmediate)
    {
        if (attachment == null)
        {
            return;
        }

        Vector3 tipWorld = transform.TransformPoint(currentTipLocal + attachmentLocalOffset);

        if (attachmentBody != null && Application.isPlaying && !forceImmediate)
        {
            attachmentBody.MovePosition(tipWorld);
            attachmentBody.MoveRotation(0f);
        }
        else
        {
            attachment.position = tipWorld;
            attachment.rotation = Quaternion.identity;
        }
    }
}
