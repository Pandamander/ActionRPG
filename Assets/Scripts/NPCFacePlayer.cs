using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFacePlayer : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;

    public float flipThreshold = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.x <= transform.position.x - flipThreshold)
        {
            spriteRenderer.flipX = true;
        } else if (playerTransform.position.x >= transform.position.x + flipThreshold)
        {
            spriteRenderer.flipX = false;
        }
    }
}
