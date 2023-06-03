using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    [SerializeField] private float floatHeight = 1f;
    [SerializeField] private float floatSpeed = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
