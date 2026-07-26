using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingPickupText : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private float fontSize = 6f;
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private float floatDistance = 2f;
    [SerializeField] private float duration = 2f;
    [SerializeField] [Range(0f, 1f)] private float fadeStartNormalized = 0.6f;
    [SerializeField] private int sortingOrder = 20;

    [Header("Outline")]
    [SerializeField] private bool enableOutline = true;
    [SerializeField] private Color outlineColor = Color.black;
    [SerializeField] [Range(0f, 1f)] private float outlineWidth = 0.2f;

    private static FloatingPickupText activeInstance;

    private TextMeshPro textMesh;
    private int displayedAmount;
    private bool isFading;

    public static void Show(FloatingPickupText prefab, Vector3 worldPosition, int amount)
    {
        if (prefab == null || amount <= 0)
        {
            return;
        }

        if (activeInstance != null && !activeInstance.isFading)
        {
            activeInstance.AddAmount(amount);
            return;
        }

        FloatingPickupText instance = Instantiate(prefab, worldPosition, Quaternion.identity);
        instance.Play(amount);
    }

    public void Play(int amount)
    {
        activeInstance = this;
        displayedAmount = amount;
        isFading = false;

        EnsureTextMesh();
        textMesh.text = $"+{displayedAmount}";
        textMesh.color = textColor;
        ApplyOutline(1f);
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    public void AddAmount(int amount)
    {
        if (amount <= 0 || isFading)
        {
            return;
        }

        displayedAmount += amount;
        if (textMesh != null)
        {
            textMesh.text = $"+{displayedAmount}";
        }
    }

    private void OnDestroy()
    {
        if (activeInstance == this)
        {
            activeInstance = null;
        }
    }

    private void EnsureTextMesh()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null)
        {
            textMesh = gameObject.AddComponent<TextMeshPro>();
        }

        if (font != null)
        {
            textMesh.font = font;
        }

        textMesh.fontSize = fontSize;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.color = textColor;
        textMesh.sortingOrder = sortingOrder;
        textMesh.enableWordWrapping = false;
        textMesh.overflowMode = TextOverflowModes.Overflow;
        textMesh.rectTransform.sizeDelta = new Vector2(4f, 1f);
    }

    private void ApplyOutline(float alpha)
    {
        if (!enableOutline)
        {
            textMesh.outlineWidth = 0f;
            Material disabledMaterial = textMesh.fontMaterial;
            if (disabledMaterial != null && disabledMaterial.HasProperty(ShaderUtilities.ID_FaceDilate))
            {
                disabledMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0f);
            }
            return;
        }

        textMesh.outlineWidth = outlineWidth;
        textMesh.outlineColor = new Color(outlineColor.r, outlineColor.g, outlineColor.b, outlineColor.a * alpha);

        Material material = textMesh.fontMaterial;
        if (material != null && material.HasProperty(ShaderUtilities.ID_FaceDilate))
        {
            material.SetFloat(ShaderUtilities.ID_FaceDilate, outlineWidth);
        }
    }

    private IEnumerator Animate()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + Vector3.up * floatDistance;
        Color startColor = textMesh.color;
        float elapsed = 0f;
        float safeDuration = Mathf.Max(0.01f, duration);

        while (elapsed < safeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / safeDuration);
            float easedT = 1f - (1f - t) * (1f - t);

            transform.position = Vector3.LerpUnclamped(startPosition, endPosition, easedT);

            float alpha = 1f;
            if (t >= fadeStartNormalized)
            {
                isFading = true;
                float fadeT = Mathf.InverseLerp(fadeStartNormalized, 1f, t);
                alpha = 1f - fadeT;
            }

            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            ApplyOutline(alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
