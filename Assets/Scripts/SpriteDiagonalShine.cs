using UnityEngine;

/// <summary>
/// Applies per-renderer overrides for the ActionRPG/Sprite Diagonal Shine material
/// so multiple objects can share one material asset with different properties.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[ExecuteAlways]
public class SpriteDiagonalShine : MonoBehaviour
{
    private static readonly int ShineColorId = Shader.PropertyToID("_ShineColor");
    private static readonly int ShineIntensityId = Shader.PropertyToID("_ShineIntensity");
    private static readonly int ShineWidthId = Shader.PropertyToID("_ShineWidth");
    private static readonly int ShineIntervalId = Shader.PropertyToID("_ShineInterval");
    private static readonly int ShineDurationId = Shader.PropertyToID("_ShineDuration");

    [SerializeField] private Color _shineColor = Color.white;
    [SerializeField, Range(0f, 2f)] private float _shineIntensity = 0.75f;
    [SerializeField, Range(0.01f, 0.5f)] private float _shineWidth = 0.12f;
    [SerializeField, Range(1f, 5f)] private float _shineInterval = 2.5f;
    [SerializeField, Range(0.1f, 2f)] private float _shineDuration = 0.65f;

    private SpriteRenderer _spriteRenderer;
    private MaterialPropertyBlock _propertyBlock;

    private void OnEnable()
    {
        Apply();
    }

    private void OnValidate()
    {
        Apply();
    }

    public void Apply()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (_spriteRenderer == null)
        {
            return;
        }

        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        _spriteRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(ShineColorId, _shineColor);
        _propertyBlock.SetFloat(ShineIntensityId, _shineIntensity);
        _propertyBlock.SetFloat(ShineWidthId, _shineWidth);
        _propertyBlock.SetFloat(ShineIntervalId, _shineInterval);
        _propertyBlock.SetFloat(ShineDurationId, _shineDuration);
        _spriteRenderer.SetPropertyBlock(_propertyBlock);
    }
}
