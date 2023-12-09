using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class IntroWreckedShipDialogue : MonoBehaviour
{
    public GameObject Player;
    [SerializeField] private DialogueSystemTrigger dialogueTrigger;
    private Animator animator;
    private PlayerMovement movement;

    private void Awake()
    {
        movement = Player.GetComponent<PlayerMovement>();
        animator = Player.GetComponent<Animator>();
    }

    void Start()
    {
        if (!OverworldSubzoneContainer.HasShownWreckedShipIntro)
        {
            OverworldSubzoneContainer.HasShownWreckedShipIntro = true;
            movement.Stop();
            animator.SetBool("IsCollapsed", true);
            StartCoroutine(StartSequence());
        }
    }

    private IEnumerator StartSequence()
    {
        yield return StartCoroutine(GetUpToCrochIdle());

        yield return new WaitForSeconds(1f);
        // Now do dialogue
        dialogueTrigger.OnUse(transform);
    }
    private IEnumerator GetUpToCrochIdle()
    {
        yield return new WaitForSeconds(4);
        animator.SetBool("IsCollapsed", false);
        animator.SetBool("IsGettingUp", true);
        yield return new WaitForSeconds(2.0f);
        animator.SetBool("IsCrouchIdling", true);
    }

    // Called by `DialogueSystemTrigger.OnConversationEnd`
    public void StandUp()
    {
        animator.SetBool("IsStandingUp", true);
        movement.AllowMovement();
    }
}
