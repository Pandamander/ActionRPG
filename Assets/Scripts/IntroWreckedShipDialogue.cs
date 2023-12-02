using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroWreckedShipDialogue : MonoBehaviour
{
    public GameObject Player;
    private Animator animator;
    private PlayerMovement movement;

    private void Awake()
    {
        movement = Player.GetComponent<PlayerMovement>();
        animator = Player.GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!OverworldSubzoneContainer.HasShownWreckedShipIntro)
        {
            OverworldSubzoneContainer.HasShownWreckedShipIntro = true;
            movement.Stop();
            animator.SetBool("IsCollapsed", true);
            StartCoroutine(GetUp());
        }
    }

    private IEnumerator GetUp()
    {
        yield return new WaitForSeconds(5);
        animator.SetBool("IsCollapsed", false);
        animator.SetBool("IsGettingUp", true);
        yield return new WaitForSeconds(0.667f);
        animator.SetBool("IsCrouchIdling", true);
        yield return new WaitForSeconds(3f);
        animator.SetBool("IsStandingUp", true);
        movement.AllowMovement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
