using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed;
    private Transform movePoint;
    private Animator animator;
    private bool isMovingTarget = false;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        rigidBody.transform.position = new Vector3(
            OverworldSubzoneContainer.LastEncounterPosition.Item1,
            OverworldSubzoneContainer.LastEncounterPosition.Item2
        );

        movePoint = transform.Find("MovePoint");
        movePoint.parent = null;

        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("speed", 1);
    }

    void Update()
    {
        float horizontal = 0;
        float vertical = 0;
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            vertical = 1;
        }
        else if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
        {
            vertical = -1;
        }
        else if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
        {
            horizontal = -1;
        }
        else if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
        {
            horizontal = 1;
        }

        Vector2 movement = new Vector2(
            horizontal,
            vertical
        );

        if (movement.x != 0 || movement.y != 0)
        {
            UpdateAnimation(movement);
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
                movement.y, 0.0f
            );
            isMovingTarget = true;
        }
        else
        {
            rigidBody.transform.position = Vector3.MoveTowards(
                transform.position,
                movePoint.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    private void UpdateAnimation(Vector2 currentMovement)
    {
        animator.SetFloat("horizontal", currentMovement.x);
        animator.SetFloat("vertical", currentMovement.y);
    }
}