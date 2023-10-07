using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsBossStateMachine : MonoBehaviour
{
    private enum BossState { NotStarted, Attacking, Kneeling, FinalBlow }

    [SerializeField] private Cyclops cyclops;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GhostTrail playerGhostTrail;
    [SerializeField] private PlayerMovement playerMovement;
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
    private float playerDeathBlowStartingX;
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
                if (cyclops.health == 0)
                {
                    bossState = BossState.FinalBlow;
                }
                break;
            case BossState.FinalBlow:
                if (!startedDeathSequence) {
                    startedDeathSequence = true;
                    Debug.Log("Start Death Sequence!");
                    playerDeathBlowStartingX = playerRb.position.x;
                    playerGhostTrail.StartTrail();
                    playerMovement.DoJump();
                } else
                {
                    if (!playerMovement.grounded)
                    {
                        playerRb.velocity = new Vector2(-15f, playerRb.velocity.y);
                    }
                }
                break;
        }
    }

    private IEnumerator DeathBlow()
    {
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator MoveToPosition(Rigidbody2D rb, Vector3 target)
    {
        float t = 0;
        Vector3 start = rb.position;

        while (t <= 1)
        {
            t += Time.fixedDeltaTime * 0.3f;
            rb.MovePosition(Vector3.Lerp(start, target, t));

            yield return null;
        }
    }
}
