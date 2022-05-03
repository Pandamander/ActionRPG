using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GOUI : MonoBehaviour
{
    private float displayTimer = 0.0f;
    private float textTimer = 0.0f;
    public TMP_Text goText;

    // Update is called once per frame
    void Update()
    {
        displayTimer += Time.deltaTime;
        if (displayTimer >= 3.0f)
            gameObject.SetActive(false);

        textTimer += Time.deltaTime;
        if (textTimer >= 0.5f)
        {
            goText.gameObject.SetActive(!goText.gameObject.activeInHierarchy);
            textTimer = 0.0f;
        }
    }
}
