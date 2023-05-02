using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TooltipInteraction : MonoBehaviour
{
    [SerializeField] public GameObject toolTipIcon;

    private bool isInTrigger = false;
    
    // Start is called before the first frame update
    void Start()
    {
        toolTipIcon.SetActive(false);
    }

    void Update()
    {
        if (isInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            DialogueSystemTrigger dialogueTrigger = FindObjectOfType<DialogueSystemTrigger>();
            if (dialogueTrigger != null)
            {
                dialogueTrigger.OnUse(transform);

                toolTipIcon.SetActive(false);
            }
        }
    }

    // When the player enters the trigger, the tooltip icon is enabled
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered");
        // if the tag is "OverworldHero" or "Hero", the tooltip icon is enabled
        if (other.CompareTag("OverworldHero") || other.CompareTag("Hero"))
        {
            isInTrigger = true;
            toolTipIcon.SetActive(true);
        }
    }

    // When the player exits the trigger, the tooltip icon is disabled
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("OverworldHero") || other.CompareTag("Hero"))
        {
            isInTrigger = false;
            toolTipIcon.SetActive(false);
        }
    }




}
