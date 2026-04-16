using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentBossAnimatorBridge : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private bool biteFinished;

    public void PlayBite()
    {
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
        StartCoroutine(Complete());
    }

    private IEnumerator Complete()
    {
        yield return new WaitForSeconds(0.5f);
        biteFinished = true;
    }
}
