using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed;
    private Transform movePoint;

    private void Awake()
    {
        movePoint = transform.Find("MovePoint");
        movePoint.parent = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.1f)
            {
                float move = Input.GetAxis("Horizontal") > 0 ? 1f : -1f;
                movePoint.position += new Vector3(move, 0.0f, 0.0f);
            }
            if (Mathf.Abs(Input.GetAxis("Vertical")) >= 0.1f)
            {
                float move = Input.GetAxis("Vertical") > 0 ? 1f : -1f;
                movePoint.position += new Vector3(0.0f, move, 0.0f);
            }
        }
    }
}
