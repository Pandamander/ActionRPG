using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NPCWalkLeftRight : MonoBehaviour
{
    // Customizable Variables for walking left and right
    public float speed = 1.0f;
    public float travelDistance = 1.0f;
    public float originalX;
    public SpriteRenderer spriteRenderer;

    public bool walking = true;

    void Start()
    {
        originalX = transform.position.x;
        // randomize travelDistance +/- 0.5
        travelDistance = Random.Range(travelDistance - 0.5f, travelDistance + 0.5f);
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.IsConversationActive == true)
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
            
            if (transform.position.x > originalX + travelDistance)
            {
                //transform.position = new Vector3(travelDistance, transform.position.y, transform.position.z);
                FlipDirection();
            }
            else if (transform.position.x < originalX - travelDistance)
            {
                //transform.position = new Vector3(-travelDistance, transform.position.y, transform.position.z);
                FlipDirection();
            }
        }

    }

    void FlipDirection()
    {
        speed *= -1;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        
    }
}
