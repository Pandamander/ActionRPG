using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{

    public Transform cameraTransform; // maybe make this public?
    public Vector2 parallaxEffectMultiplier; // this determines the amount of parallax

    private Vector3 lastCameraPosition;


    // Start is called before the first frame update
    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition; // the amount the camera has moved since the last frame
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y); // update the transform of the object this script is on

        lastCameraPosition = cameraTransform.position; // update last camera position for next frame update

    }
}
