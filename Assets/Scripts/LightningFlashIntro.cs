using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightningFlashIntro : MonoBehaviour
{

    private float timeBetweenFlashes = 4.0f;
    private float elapsedTime = 0;
    private SpriteRenderer lightningFlashImage;
    private bool isFirstLightningFlash = true;
    public GameObject serpentSilhouette;

    // Start is called before the first frame update
    void Start()
    {
        lightningFlashImage = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeBetweenFlashes)
        {
            // change the lightning flash sprite after the first flash to not be the serpent silhouette in the background
            if (isFirstLightningFlash)
            {
                serpentSilhouette.GetComponent<IntroShipMovement>().speed = 5.5f;
                StartCoroutine(SerpentSilhouetteFlash());
            }

            StartCoroutine(LightingFlash());
            elapsedTime = 0;
            timeBetweenFlashes = Random.Range(2.0f, 5.0f);
        }
    }

    public IEnumerator SerpentSilhouetteFlash()
    {
        SpriteRenderer serpentSilhouetteSprite = serpentSilhouette.GetComponent<SpriteRenderer>();

        serpentSilhouetteSprite.color = new Color(serpentSilhouetteSprite.color.r, serpentSilhouetteSprite.color.g, serpentSilhouetteSprite.color.b, 1.0f);

        float lerpDuration = 0.4f;
        float lerpTimeElapsed = 0;

        // fade color to 0 alpha
        while (serpentSilhouetteSprite.color.a > 0)
        {
            float lerpPercentage = lerpTimeElapsed / lerpDuration;
            lerpTimeElapsed += Time.deltaTime;
            serpentSilhouetteSprite.color = new Color(serpentSilhouetteSprite.color.r, serpentSilhouetteSprite.color.g, serpentSilhouetteSprite.color.b, Mathf.Lerp(1.0f, 0, lerpPercentage));

            yield return null;
        }

        yield return null;
    }

    public IEnumerator LightingFlash()
    {
        lightningFlashImage.color = new Color(lightningFlashImage.color.r, lightningFlashImage.color.g, lightningFlashImage.color.b, 1.0f);

        // longer delay on the first flash
        if (isFirstLightningFlash) {
            yield return new WaitForSeconds(0.3f);
        } else
        {
            yield return new WaitForSeconds(0.1f);
        }

        float lerpDuration = 0.4f;
        float lerpTimeElapsed = 0;

        // fade color to 0 alpha
        while (lightningFlashImage.color.a > 0)
        {
            float lerpPercentage = lerpTimeElapsed / lerpDuration;
            lerpTimeElapsed += Time.deltaTime;
            lightningFlashImage.color = new Color(lightningFlashImage.color.r, lightningFlashImage.color.g, lightningFlashImage.color.b, Mathf.Lerp(1.0f, 0, lerpPercentage));

            yield return null;
        }

        // set a new time between flashes
        timeBetweenFlashes = Random.Range(3.0f, 8.0f);

        if (isFirstLightningFlash)
        {
            isFirstLightningFlash = false;
        }

        yield return null;
    }
}
