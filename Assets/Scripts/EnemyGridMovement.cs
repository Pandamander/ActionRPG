using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGridMovement : MonoBehaviour
{
    public float moveSpeed;
    public Vector2[] movePath;
    public Transform movePoint;
    private Animator animator;
    private bool isMovingTarget = false;
    private int moveIndex = 0;
    private bool stopMovement = false;

    private void Awake()
    {
        movePoint = transform.Find("MovePoint");
        movePoint.parent = null;
    }

    void Update()
    {
        if (stopMovement) { return; }

        bool movementKeyPressed = false;
        if (Input.GetKeyDown("up") ||
            Input.GetKeyDown("down") ||
            Input.GetKeyDown("left") ||
            Input.GetKeyDown("right") ||
            Input.GetKeyDown("w") ||
            Input.GetKeyDown("s") ||
            Input.GetKeyDown("a") ||
            Input.GetKeyDown("d"))
        {
            movementKeyPressed = true;
        }

        Vector2 movement = new Vector2(0, 0);
        if (movementKeyPressed)
        {
            movement = movePath[moveIndex];
            moveIndex += 1;
            if (moveIndex == movePath.Length)
            {
                moveIndex = 0;
            }
        }

        float distanceToMovePoint = Vector3.Distance(
            transform.position,
            movePoint.position
        );
        if (distanceToMovePoint == 0)
        {
            isMovingTarget = false;
        }

        if (!isMovingTarget)
        {
            movePoint.position += new Vector3(
                movement.x,
                movement.y,
                0.0f
            );
            isMovingTarget = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                movePoint.position,
                moveSpeed * Time.deltaTime
            );
        }
    }
    public void StopMovement()
    {
        stopMovement = true;
    }
}
