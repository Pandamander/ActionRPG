using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentBossAnimatorBridge : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int BiteHash = Animator.StringToHash("Bite");

    private bool biteFinished;

    public void PlayBite()
    {
        Debug.Log("PlayBite");
        biteFinished = false;
        animator.SetTrigger("Bite");
    }

    public bool IsBiteFinished()
    {
        return biteFinished;
    }

    public void OnBiteAnimationFinished()
    {
        Debug.Log("OnBiteAnimationFinished");
        biteFinished = true;
    }
}
