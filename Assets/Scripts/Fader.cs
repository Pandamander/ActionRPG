using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
    }

    public void FadeOut(float step = 0.2f)
    {
        StartCoroutine(DoFadeOut(step));
    }

    public void FadeIn(float step = 0.2f)
    {
        StartCoroutine(DoFadeIn(step));
    }

    public IEnumerator DoFadeOut(float step = 0.2f)
    {
        while (image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - step);
            yield return new WaitForSeconds(0.08f);
        }
        yield return null;
    }

    public IEnumerator DoFadeIn(float step = 0.2f)
    {
        while (image.color.a < 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + step);
            yield return new WaitForSeconds(0.08f);
        }
        yield return null;
    }
}
