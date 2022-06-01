using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blink : MonoBehaviour
{

    public float blinkDelay = 1.0f;
    private float timeSinceLastBlink = 0;

    TextMeshProUGUI thisTextMesh;
    // Start is called before the first frame update
    void Start()
    {
        thisTextMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastBlink += Time.deltaTime;
        if (timeSinceLastBlink >= blinkDelay) {
            timeSinceLastBlink = 0;

            if (thisTextMesh.color.a == 0)
            {
                thisTextMesh.color = new Color(thisTextMesh.color.r, thisTextMesh.color.g, thisTextMesh.color.b, 1);
            } else if (thisTextMesh.color.a == 1)
            {
                thisTextMesh.color = new Color(thisTextMesh.color.r, thisTextMesh.color.g, thisTextMesh.color.b, 0);
            }
        }
    }
}
