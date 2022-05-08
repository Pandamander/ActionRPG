using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFollow : MonoBehaviour
{

    public Transform target;
    public float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90.0f;

        rb.AddForce(force);
        rb.MoveRotation(angle);
    }
}
