using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGLightningFlash : MonoBehaviour
{

    private float timeBetweenFlashes = 4.0f;
    private float elapsedTime = 0;
    public SpriteRenderer bgLightningFlashSprite;

    // Start is called before the first frame update
    void Start()
    {
        bgLightningFlashSprite = GetComponent<SpriteRenderer>();
        bgLightningFlashSprite.color = new Color(bgLightningFlashSprite.color.r, bgLightningFlashSprite.color.g, bgLightningFlashSprite.color.b, 0); // set initial alpha to 0
    }

    // Update is called once per frame
    //void Update()
    //{
        //elapsedTime += Time.deltaTime;

        //if (elapsedTime >= timeBetweenFlashes)
        //{
            //StartCoroutine(LightningFlash());
            //elapsedTime = 0;
            //timeBetweenFlashes = 3.0f;// Random.Range(2.0f, 5.0f);
        //}
    //}

    public void DoLightningFlash()
    {
        print("doing lightning flash");
        StopCoroutine(LightningFlash());
        StartCoroutine(LightningFlash());
    }

    private IEnumerator LightningFlash()
    {
        bgLightningFlashSprite.color = new Color(bgLightningFlashSprite.color.r, bgLightningFlashSprite.color.g, bgLightningFlashSprite.color.b, 1.0f);

        yield return new WaitForSeconds(0.1f);

        while (bgLightningFlashSprite.color.a > 0)
        {
            bgLightningFlashSprite.color = new Color(bgLightningFlashSprite.color.r, bgLightningFlashSprite.color.g, bgLightningFlashSprite.color.b, bgLightningFlashSprite.color.a - 0.2f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
