using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsBossStateMachine : MonoBehaviour
{
    private enum BossState { NotStarted, Attacking, Kneeling }

    [SerializeField] private Cyclops cyclops;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform playerDeathBlowLocation1;
    [SerializeField] private Transform playerDeathBlowLocation2;

    private BossState bossState = BossState.NotStarted;
    private float attackTimer = 2f;
    private float attackTimeCounter = 0f;
    private float moveSpeed = 2.5f;
    private float moveDirection = -1f;
    private float moveTimer = 8f;
    private float moveTimeCounter = 0f;
    private List<int> swipeHealths = new() { 11, 8, 5, 2 };
    private bool startedDeathSequence = false;
    public void Run()
    {
        cyclops.Walk();
        bossState = BossState.Attacking;
    }

    // Update is called once per frame
    void Update()
    {
        switch (bossState)
        {
            case BossState.NotStarted:
                return;
            case BossState.Attacking:
                if (cyclops.health <= 0)
                {
                    cyclops.KneelForFinalBlow();
                    bossState = BossState.Kneeling;
                }

                attackTimeCounter += Time.deltaTime;
                if (attackTimeCounter >= attackTimer)
                {
                    attackTimeCounter = 0f;
                    attackTimer = Random.Range(2.0f, 3.0f);
                    int randAttack = Random.Range(0, cyclops.autoAttackTypes.Length);
                    cyclops.Attack(cyclops.autoAttackTypes[randAttack]);
                }

                cyclops.Move(moveSpeed * moveDirection);

                moveTimeCounter += Time.deltaTime;
                if (moveTimeCounter >= moveTimer)
                {
                    moveTimeCounter = 0f;
                    moveDirection *= -1;
                }

                int foundSwipeIndex = swipeHealths.IndexOf(cyclops.health);
                if (foundSwipeIndex >= 0)
                {
                    swipeHealths.RemoveAt(foundSwipeIndex);
                    cyclops.Attack(Cyclops.AttackType.Swipe);
                }

                break;
            case BossState.Kneeling:
                if (startedDeathSequence) { return; }
                if (cyclops.health < 0)
                {
                    startedDeathSequence = true;
                    Debug.Log("Start Death Sequence!");
                }
                break;
        }
    }
}
