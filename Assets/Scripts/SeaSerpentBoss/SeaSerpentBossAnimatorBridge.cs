using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentBossAnimatorBridge : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer head;

    private bool biteFinished;
    private int sortingOrder;

    private void Awake()
    {
        sortingOrder = head.sortingOrder;
    }

    public void PlayBite()
    {
        biteFinished = false;
        head.sortingOrder = 12;
        animator.SetTrigger("Bite");
    }

    public bool IsBiteFinished()
    {
        return biteFinished;
    }

    public void OnBiteEnd()
    {
        Debug.Log("OnBiteAnimationFinished");
        StartCoroutine(Complete());
    }

    private IEnumerator Complete()
    {
        yield return new WaitForSeconds(0.5f);
        head.sortingOrder = sortingOrder;
        biteFinished = true;
    }
}
