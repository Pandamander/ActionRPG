using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BreakableDebris : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite[] sprites;

    [Header("Fade")]
    [SerializeField] private float fadeDelay = 0.5f;
    [SerializeField] private float fadeDuration = 0.5f;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private float elapsed;
    private bool fading;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyRandomSprite();
    }

    public void Initialize(Vector2 velocity, float gravityScale, float angularVelocity)
    {
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        rigidBody.gravityScale = gravityScale;
        rigidBody.velocity = velocity;
        rigidBody.angularVelocity = angularVelocity;
    }

    private void ApplyRandomSprite()
    {
        if (sprites == null || sprites.Length == 0)
        {
            return;
        }

        Sprite chosen = sprites[Random.Range(0, sprites.Length)];
        if (chosen != null)
        {
            spriteRenderer.sprite = chosen;
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if (!fading)
        {
            if (elapsed < fadeDelay)
            {
                return;
            }

            fading = true;
            elapsed = 0f;
            return;
        }

        if (fadeDuration <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        float t = Mathf.Clamp01(elapsed / fadeDuration);
        Color color = spriteRenderer.color;
        color.a = 1f - t;
        spriteRenderer.color = color;

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
