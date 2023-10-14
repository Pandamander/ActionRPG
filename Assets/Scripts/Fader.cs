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

    public void FadeOut()
    {
        StartCoroutine(DoFadeOut());
    }

    public void FadeIn()
    {
        StartCoroutine(DoFadeIn());
    }

    public IEnumerator DoFadeOut()
    {
        while (image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.2f);
            yield return new WaitForSeconds(0.08f);
        }
        yield return null;
    }

    public IEnumerator DoFadeIn()
    {
        while (image.color.a < 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.2f);
            yield return new WaitForSeconds(0.08f);
        }
        yield return null;
    }
}
