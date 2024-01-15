using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightningFlash : MonoBehaviour
{

    private float timeBetweenFlashes = 4.0f;
    private float elapsedTime = 0;
    private Image lightningFlashImage;

    // Start is called before the first frame update
    void Start()
    {
        lightningFlashImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeBetweenFlashes)
        {
            StartCoroutine(LightingFlash());
            elapsedTime = 0;
            timeBetweenFlashes = Random.Range(2.0f, 5.0f);
        }
    }

    public IEnumerator LightingFlash()
    {
        lightningFlashImage.color = new Color(lightningFlashImage.color.r, lightningFlashImage.color.g, lightningFlashImage.color.b, 1.0f);

        yield return new WaitForSeconds(0.1f);

        float lerpDuration = 0.4f;
        float lerpTimeElapsed = 0;

        // fade color to 0 alpha
        while (lightningFlashImage.color.a > 0)
        {
            float lerpPercentage = lerpTimeElapsed / lerpDuration;
            //Mathf.Lerp(1.0f, 0, lerpPercentage);
            lerpTimeElapsed += Time.deltaTime;
            lightningFlashImage.color = new Color(lightningFlashImage.color.r, lightningFlashImage.color.g, lightningFlashImage.color.b, Mathf.Lerp(1.0f, 0, lerpPercentage));
            yield return null;
        }

        // set a new time between flashes
        timeBetweenFlashes = Random.Range(3.0f, 8.0f);

        yield return null;
    }
}
