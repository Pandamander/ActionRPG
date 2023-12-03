using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 1.2f;

    private float xDirection = -1.0f;
    private float yDirection = -1.0f;

    private Vector3 direction;

    private void Start()
    {
        direction = new Vector3(xDirection, yDirection);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * (Time.deltaTime * speed));
    }
}
