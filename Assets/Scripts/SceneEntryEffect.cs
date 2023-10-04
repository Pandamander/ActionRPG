using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneEntryEffect : MonoBehaviour
{

    [SerializeField] GameObject faderObject;
    private Image image;

    
    void Start()
    {
        if (faderObject != null)
        {
            image = faderObject.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
            StartCoroutine(DoSceneEntryEffect());
        }
    }

    private IEnumerator DoSceneEntryEffect()
    {

        if (image != null)
        {
            while (image.color.a > 0)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.2f);
                yield return new WaitForSeconds(0.08f);
            }
        }

        yield return null;
    }
}
