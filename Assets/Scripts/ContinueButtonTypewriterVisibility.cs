using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Hides the Continue button graphic after the typewriter finishes on the
/// final conversation line (no further responses). Keeps the Button interactive.
/// </summary>
[RequireComponent(typeof(Image))]
public class ContinueButtonTypewriterVisibility : MonoBehaviour
{
    [SerializeField] private AbstractTypewriterEffect typewriterEffect;
    [SerializeField] private Animator continueAnimator;
    [SerializeField] private CanvasGroup canvasGroup;

    private UnityEvent onBegin;
    private UnityEvent onEnd;

    private void Awake()
    {
        if (continueAnimator == null)
        {
            continueAnimator = GetComponent<Animator>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        // Keep raycasts so mouse/E-key input still works while the graphic is hidden.
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        ResolveTypewriter();
        CacheTypewriterEvents();
    }

    private void OnEnable()
    {
        Subscribe();
        // Visible when the button appears. Hide only from onEnd on the final line
        // so a delayed typewriter start cannot hide the graphic early.
        SetGraphicVisible(true);
    }

    private void OnDisable()
    {
        Unsubscribe();
        SetGraphicVisible(true);
    }

    private void ResolveTypewriter()
    {
        if (typewriterEffect != null)
        {
            return;
        }

        var fastForward = GetComponent<StandardUIContinueButtonFastForward>();
        if (fastForward != null && fastForward.typewriterEffect != null)
        {
            typewriterEffect = fastForward.typewriterEffect;
            return;
        }

        var panel = GetComponentInParent<StandardUISubtitlePanel>();
        if (panel != null)
        {
            typewriterEffect = panel.GetTypewriter();
        }
    }

    private void CacheTypewriterEvents()
    {
        onBegin = null;
        onEnd = null;

        if (typewriterEffect == null)
        {
            return;
        }

        // Project uses Unity UI typewriter (TMP typewriter is a stub without TMP_PRESENT).
        var unityTypewriter = typewriterEffect as UnityUITypewriterEffect;
        if (unityTypewriter != null)
        {
            onBegin = unityTypewriter.onBegin;
            onEnd = unityTypewriter.onEnd;
        }
    }

    private void Subscribe()
    {
        if (onBegin != null)
        {
            onBegin.AddListener(OnTypewriterBegin);
        }

        if (onEnd != null)
        {
            onEnd.AddListener(OnTypewriterEnd);
        }
    }

    private void Unsubscribe()
    {
        if (onBegin != null)
        {
            onBegin.RemoveListener(OnTypewriterBegin);
        }

        if (onEnd != null)
        {
            onEnd.RemoveListener(OnTypewriterEnd);
        }
    }

    private void OnTypewriterBegin()
    {
        SetGraphicVisible(true);
    }

    private void OnTypewriterEnd()
    {
        SetGraphicVisible(!IsLastLineOfConversation());
    }

    private static bool IsLastLineOfConversation()
    {
        if (!DialogueManager.isConversationActive || DialogueManager.currentConversationState == null)
        {
            return false;
        }

        return !DialogueManager.currentConversationState.hasAnyResponses;
    }

    private void SetGraphicVisible(bool visible)
    {
        // CanvasGroup alpha hides the sprite without fighting Button Color Tint,
        // which continuously overwrites Image.color.
        if (canvasGroup != null)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
        }

        if (continueAnimator != null && continueAnimator.enabled != visible)
        {
            continueAnimator.enabled = visible;
        }
    }
}
