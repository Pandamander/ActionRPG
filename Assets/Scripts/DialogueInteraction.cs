using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DialogueInteraction : MonoBehaviour
{
    private bool isInTrigger = false;

    void Update()
    {
        if (isInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            DialogueSystemTrigger dialogueTrigger = FindObjectOfType<DialogueSystemTrigger>();
            if (dialogueTrigger != null)
            {
                dialogueTrigger.OnUse(transform);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OverworldHero") || other.CompareTag("Hero"))
        {
            isInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("OverworldHero") || other.CompareTag("Hero"))
        {
            isInTrigger = false;
        }
    }
}
