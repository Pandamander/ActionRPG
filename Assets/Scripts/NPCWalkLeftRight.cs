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
        }
        else
        {
            walking = true;
        }

        // If walking, move the NPC left and right based on the speed and travel distance
        if (walking)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            animator.SetBool("isWalking", true);

            if (transform.position.x > originalX + travelDistance)
            {
                FlipDirection();
            }
            else if (transform.position.x < originalX - travelDistance)
            {
                FlipDirection();
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

    }

    void FlipDirection()
    {
        speed *= -1;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
