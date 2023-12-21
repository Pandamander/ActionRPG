using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;

    private float xDirection = -1.0f;
    private float yDirection = -1.0f;

    private Vector3 direction;

    private void Start()
    {
        speed = Random.Range(0.03f, 0.15f);
        direction = new Vector3(xDirection, yDirection);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * (Time.deltaTime * speed));
    }
}
