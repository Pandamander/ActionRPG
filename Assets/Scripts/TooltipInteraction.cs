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

        // Lua.RegisterFunction("HelloWorld", this, SymbolExtensions.GetMethodInfo(() => HelloWorld())); // Register the function to Lua
    }

    void Update()
    {
        if (isInTrigger && Input.GetKeyDown(KeyCode.E) && DialogueManager.IsConversationActive == false)
        {
            // Get the dialogue trigger component from this gameObject
            DialogueSystemTrigger dialogueTrigger = this.GetComponent<DialogueSystemTrigger>();
            if (dialogueTrigger != null)
            {
                dialogueTrigger.OnUse(transform);
                toolTipIcon.SetActive(false);

                int XP = PixelCrushers.DialogueSystem.DialogueLua.GetVariable("XP").asInt;
                Debug.Log("XP: " + XP);
                PixelCrushers.DialogueSystem.DialogueLua.SetVariable("XP", XP + 100);

            }
        }
    }

    // When the player enters the trigger, the tooltip icon is enabled
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the tag is "OverworldHero" or "Hero", the tooltip icon is enabled
        if (other.CompareTag("OverworldHero") || other.CompareTag("Hero") || other.CompareTag("Player"))
        {
            isInTrigger = true;
            toolTipIcon.SetActive(true);
        }
    }

    // When the player exits the trigger, the tooltip icon is disabled
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("OverworldHero") || other.CompareTag("Hero") || other.CompareTag("Player"))
        {
            isInTrigger = false;
            toolTipIcon.SetActive(false);
        }
    }



}
