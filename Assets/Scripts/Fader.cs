using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private Image image;
    public bool doSteppedFade = true;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
    }

    private void Start()
    {
        if (doSteppedFade)
        {
            SteppedFadeOut();
        } else
        {
            SmoothFadeOut();
        }
    }

    public void SmoothFadeOut()
    {
        StartCoroutine(DoSmoothFadeOut(2.0f));
    }

    public void SmoothFadeIn()
    {
        StartCoroutine(DoSmoothFadeIn(2.0f));
    }

    public IEnumerator DoSmoothFadeOut(float duration)
    {
        float elapsedTime = 0;
        while (image.color.a > 0)
        {
            float newAlpha = Mathf.Lerp(1.0f, 0, elapsedTime / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    public IEnumerator DoSmoothFadeIn(float duration)
    {
        float elapsedTime = 0;
        while (image.color.a < 1.0f)
        {
            float newAlpha = Mathf.Lerp(0, 1.0f, elapsedTime / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    public void SteppedFadeOut(float step = 0.2f)
    {
        StartCoroutine(DoSteppedFadeOut(step));
    }

    public void SteppedFadeIn(float step = 0.2f)
    {
        StartCoroutine(DoSteppedFadeIn(step));
    }

    public IEnumerator DoSteppedFadeOut(float step = 0.2f)
    {
        while (image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - step);
            yield return new WaitForSeconds(0.08f);
        }
        yield return null;
    }

    public IEnumerator DoSteppedFadeIn(float step = 0.2f)
    {
        while (image.color.a < 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + step);
            yield return new WaitForSeconds(0.08f);
        }
        yield return null;
    }
}
