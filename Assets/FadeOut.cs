using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);

        StartCoroutine(DoFadeOut());
    }

    private IEnumerator DoFadeOut()
    {
        while (image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.2f);
            yield return new WaitForSeconds(0.08f);
        }
        yield return null;
    }
}
