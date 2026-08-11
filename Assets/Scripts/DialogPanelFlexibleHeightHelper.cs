using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Measures dialog height at the final width (with full subtitle text), locks that height,
/// then plays the panel show animation so horizontal expand does not reflow/jerk height
/// via Content Size Fitter.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class DialogPanelFlexibleHeightHelper : MonoBehaviour
{
    [SerializeField] private float finalSizeDeltaX = -224f;
    [SerializeField] private float startSizeDeltaX = -434f;
    [SerializeField] private string showTriggerName = "Show";
    [Tooltip("Frames to wait after open so subtitle text/typewriter can populate before measuring.")]
    [SerializeField] private int settleFrames = 1;

    private RectTransform rectTransform;
    private ContentSizeFitter contentSizeFitter;
    private LayoutElement layoutElement;
    private Animator animator;
    private UIPanel uiPanel;
    private StandardUISubtitlePanel subtitlePanel;

    private float lockedHeight;
    private bool heightLocked;
    private Coroutine prepareCoroutine;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null)
        {
            layoutElement = gameObject.AddComponent<LayoutElement>();
        }

        animator = GetComponent<Animator>();
        uiPanel = GetComponent<UIPanel>();
        subtitlePanel = GetComponent<StandardUISubtitlePanel>();

        // Open() runs before subtitle text is set. Take ownership of the show trigger
        // so we can measure at full width first, then expand.
        if (uiPanel != null)
        {
            if (string.IsNullOrEmpty(showTriggerName) && !string.IsNullOrEmpty(uiPanel.showAnimationTrigger))
            {
                showTriggerName = uiPanel.showAnimationTrigger;
            }

            uiPanel.showAnimationTrigger = string.Empty;
            uiPanel.onOpen.AddListener(OnPanelOpen);
            uiPanel.onClose.AddListener(OnPanelClose);
        }
    }

    private void OnDestroy()
    {
        if (uiPanel == null)
        {
            return;
        }

        uiPanel.onOpen.RemoveListener(OnPanelOpen);
        uiPanel.onClose.RemoveListener(OnPanelClose);
    }

    private void LateUpdate()
    {
        // Animator Write Defaults + clips that only key SizeDelta.x can overwrite height.
        if (heightLocked)
        {
            ApplyLockedHeight();
        }
    }

    private void OnPanelOpen()
    {
        if (prepareCoroutine != null)
        {
            StopCoroutine(prepareCoroutine);
        }

        prepareCoroutine = StartCoroutine(PrepareThenPlayShow());
    }

    private void OnPanelClose()
    {
        if (prepareCoroutine != null)
        {
            StopCoroutine(prepareCoroutine);
            prepareCoroutine = null;
        }

        UnlockHeight();
    }

    private IEnumerator PrepareThenPlayShow()
    {
        int frames = Mathf.Max(0, settleFrames);
        for (int i = 0; i < frames; i++)
        {
            yield return null;
        }

        // Typewriter may need an extra frame to place full text with transparent remainder.
        int safety = 0;
        while (string.IsNullOrEmpty(GetSubtitleText()) && safety < 8)
        {
            safety++;
            yield return null;
        }

        MeasureAndLockHeight();
        PlayShowAnimation();
        prepareCoroutine = null;
    }

    private string GetSubtitleText()
    {
        if (subtitlePanel == null || subtitlePanel.subtitleText == null)
        {
            return null;
        }

        return subtitlePanel.subtitleText.text;
    }

    private void MeasureAndLockHeight()
    {
        if (animator != null && !string.IsNullOrEmpty(showTriggerName))
        {
            animator.ResetTrigger(showTriggerName);
        }

        if (contentSizeFitter != null)
        {
            contentSizeFitter.enabled = true;
        }

        Vector2 size = rectTransform.sizeDelta;
        size.x = finalSizeDeltaX;
        rectTransform.sizeDelta = size;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        float height = rectTransform.rect.height;
        if (height <= 0.01f)
        {
            height = LayoutUtility.GetPreferredHeight(rectTransform);
        }

        lockedHeight = height;
        heightLocked = true;

        layoutElement.minHeight = lockedHeight;
        layoutElement.preferredHeight = lockedHeight;

        if (contentSizeFitter != null)
        {
            contentSizeFitter.enabled = false;
        }

        size.x = startSizeDeltaX;
        size.y = lockedHeight;
        rectTransform.sizeDelta = size;
    }

    private void ApplyLockedHeight()
    {
        Vector2 size = rectTransform.sizeDelta;
        if (!Mathf.Approximately(size.y, lockedHeight))
        {
            size.y = lockedHeight;
            rectTransform.sizeDelta = size;
        }
    }

    private void PlayShowAnimation()
    {
        if (animator == null || string.IsNullOrEmpty(showTriggerName))
        {
            return;
        }

        if (!animator.enabled)
        {
            animator.enabled = true;
        }

        animator.SetTrigger(showTriggerName);
    }

    private void UnlockHeight()
    {
        heightLocked = false;
        lockedHeight = 0f;

        if (layoutElement != null)
        {
            layoutElement.minHeight = -1f;
            layoutElement.preferredHeight = -1f;
        }

        if (contentSizeFitter != null)
        {
            contentSizeFitter.enabled = true;
        }
    }
}
