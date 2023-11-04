using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroShipMovement : MonoBehaviour
{
    [SerializeField] private float floatHeight = 1f;
    [SerializeField] private float floatSpeed = 1f;
    public float speed;

    Quaternion startRot = Quaternion.Euler(0, 0, -5.0f);
    Quaternion endRot = Quaternion.Euler(0, 0, 5.0f);

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float rawHeightValue = Mathf.Sin(Time.time * floatSpeed) * floatHeight; // this produces a range of values from -floatHeight to floatHeight
        float rawRotationValue = Mathf.Sin(Time.time * floatSpeed + 1.8f) * floatHeight; // this is taking the same value as above and phase shifting it so the ship's rotation input is slightly delayed from the ship's y position

        float newY = startPosition.y + rawHeightValue; // this is setting the value for the y position of the ship

        float scaledRotationValue = (rawRotationValue + 0.5f); // this is constraining the rotation value calculated above to a 0 to 1 range, which is used in the Lerp below as the interpolation parameter
        transform.rotation = Quaternion.Lerp(startRot, endRot, scaledRotationValue); // this is setting the rotation of the ship

        transform.position = new Vector3(transform.position.x + (Time.deltaTime * speed), newY, transform.position.z); // finally, this line sets the ship's position and moves it to the right
    }
}
