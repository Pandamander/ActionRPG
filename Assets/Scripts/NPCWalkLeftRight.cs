using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NPCWalkLeftRight : MonoBehaviour
{
    // Customizable Variables for walking left and right
    public float speed = 1.0f;
    public float travelDistance = 1.0f;

    private float originalX;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public bool walking = true;

    void Awake()
    {
        originalX = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only stop the NPC being conversed with
        if (DialogueManager.IsConversationActive == true &&
            DialogueManager.currentConversant.gameObject.CompareTag(gameObject.tag))
        {
            walking = false;
            animator.SetBool("isWalking", false);
            FacePlayer();
        }
        else
        {
            walking = true;
            animator.SetBool("isWalking", true);
        }

        // If walking, move the NPC left and right based on the speed and travel distance
        if (walking)
        {
            FaceWalkingDirection();

            transform.position += Vector3.right * speed * Time.deltaTime;

            if ((transform.position.x >= originalX + travelDistance) && !spriteRenderer.flipX)
            {
                FlipDirection();
            }
            else if ((transform.position.x <= originalX - travelDistance) && spriteRenderer.flipX)
            {
                FlipDirection();
            }
        }

    }

    void FacePlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject.transform.position.x < transform.position.x)
        {
            // face left
            spriteRenderer.flipX = true;
        } else if (playerObject.transform.position.x > transform.position.x)
        {
            // face right
            spriteRenderer.flipX = false;
        }
    }

    void FaceWalkingDirection()
    {
        // right
        if (speed > 0)
        {
            spriteRenderer.flipX = false;
        }
        // left
        else if (speed < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void FlipDirection()
    {
        speed *= -1;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
