using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    private bool isFlickering = false;
    private float timeDelay;
    // Start is called before the first frame update

    IEnumerator FlickerLight2()
    {
        isFlickering = true;
        gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = true;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFlickering)
        {
            StartCoroutine(FlickerLight2());
        }
    }
}
