using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class AdvanceDialogueOnKeyPress : MonoBehaviour
{

    //Overall this class is not working yet. It's supposed to advance dialog
    public KeyCode advanceKey = KeyCode.Space;
    private DialogueSystemController myDialogueSystemController;

    // Start is called before the first frame update
    void Start()
    {
        myDialogueSystemController = GetComponent<DialogueSystemController>();
    }

    void Update()
    {
        // Looks like you can reference either DialogueManager, which is a static class available everywhere, or GetComponent<DialogueSystemController>()
        
        if (DialogueManager.IsConversationActive && Input.GetKeyDown(advanceKey))
        {
            Debug.Log("PRES"); // This is working

            //Advance the conversation from the Dialogue Manager
            //DialogueManager.Instance.SendMessage("OnContinue");
            myDialogueSystemController.StopConversation();
        }
    }
}