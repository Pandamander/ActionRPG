using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float moveSpeed;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private bool stopMovement = false;
    private Vector2 movement;

    private void Awake()
    {
        // Set position to last encounter
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        rigidBody.transform.position = new Vector3(
            OverworldSubzoneContainer.LastEncounterPosition.Item1,
            OverworldSubzoneContainer.LastEncounterPosition.Item2 - 1 // Offset player from last encounter so we don't auto-collide again.
        );

        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("speed", 1);
    }

    void Update()
    {
        if (stopMovement) { return; }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Vector2.SqrMagnitude(movement) > 0f)
        {
            UpdateAnimation(movement);
        }
    }

    private void FixedUpdate()
    {
        //rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
        rigidBody.velocity = movement * moveSpeed;
    }

    private void UpdateAnimation(Vector2 currentMovement)
    {
        animator.SetFloat("horizontal", currentMovement.x);
        animator.SetFloat("vertical", currentMovement.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void StopMovement()
    {
        stopMovement = true;
    }
}
