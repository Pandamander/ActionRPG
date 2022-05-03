using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<CameraShake>().ShakeCamera(2.0f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
