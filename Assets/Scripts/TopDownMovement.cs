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
            OverworldSubzoneContainer.LastEncounterPosition.Item2
        );

        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("speed", 1);

        switch (OverworldSubzoneContainer.LastEncounterDirection)
        {
            case OverworldSubzoneContainer.PlayerDirection.Up:
                animator.SetFloat("vertical", 1);
                break;
            case OverworldSubzoneContainer.PlayerDirection.Down:
                animator.SetFloat("vertical", -1);
                break;
            case OverworldSubzoneContainer.PlayerDirection.Left:
                animator.SetFloat("horizontal", 1);
                break;
            case OverworldSubzoneContainer.PlayerDirection.Right:
                animator.SetFloat("horizontal", -1);
                break;
        }
    }

    void Update()
    {
        if (stopMovement) {
            movement = Vector2.zero;
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(movement.x) >= 1f)
        {
            movement.y = 0f;
            UpdateAnimation(movement);
        }
        if (Mathf.Abs(movement.y) >= 1f)
        {
            movement.x = 0f;
            UpdateAnimation(movement);
        }

        // Enforce single direction movement on gamepad
        if (Mathf.Abs(movement.x) < 1f)
        {
            movement.x = 0f;
        }
        if (Mathf.Abs(movement.y) < 1f)
        {
            movement.y = 0f;
        }
    }

    private void FixedUpdate()
    {
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
        animator.speed = 0;
    }
}
