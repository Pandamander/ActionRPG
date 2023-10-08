using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsBossStateMachine : MonoBehaviour
{
    public enum BossState { NotStarted, Attacking, Kneeling, FinalBlow, Dead }

    [SerializeField] private Cyclops cyclops;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GhostTrail playerGhostTrail;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform playerDeathBlowLocation2;
    [SerializeField] private SubzoneAudioManager audioManager;
    [SerializeField] private CameraShake cameraShake;

    public BossState bossState { get; private set; } = BossState.NotStarted;
    private float attackTimer = 2f;
    private float attackTimeCounter = 0f;
    private float moveSpeed = 2.5f;
    private float moveDirection = -1f;
    private float moveTimer = 8f;
    private float moveTimeCounter = 0f;
    private List<int> swipeHealths = new() { 11, 8, 5, 2 };
    private bool startedDeathSequence = false;
    private bool beginCheckingForGrounded = false;
    private bool beginCheckingForFinalPosition = false;
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
                if (cyclops.health == 0)
                {
                    cyclops.KneelForFinalBlow();
                    bossState = BossState.Kneeling;
                    return;
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
                    audioManager.StopMusic();
                    StartCoroutine(JumpBackForDeathBlow());
                }
                if (beginCheckingForGrounded)
                {
                    if (playerRb.position.y < -2.5f)
                    {
                        beginCheckingForGrounded = false;
                        beginCheckingForFinalPosition = true;
                        playerMovement.Stop();
                        StartCoroutine(DeathBlow());
                    }
                }
                if (beginCheckingForFinalPosition)
                {
                    if (playerRb.position.x >= playerDeathBlowLocation2.position.x)
                    {
                        beginCheckingForFinalPosition = false;
                        playerRb.gravityScale = 0f;
                        StartCoroutine(CyclopsDie());
                    }
                }
                break;
        }
    }

    private IEnumerator CyclopsDie()
    {
        cameraShake.ShakeCamera(3.5f, 5f);
        audioManager.PlayExplosion();
        cyclops.Die();
        yield return new WaitForSeconds(3.5f);
        Destroy(cyclops.gameObject);
        playerGhostTrail.StopTrail();
        playerRb.gravityScale = 5f;
        playerAnimator.SetBool("IsAttacking", false);
        playerMovement.AllowMovement();
        bossState = BossState.Dead;
    }

    private IEnumerator DeathBlow()
    {
        yield return new WaitForSeconds(1f);
        playerAnimator.SetBool("IsCrouching", false);
        playerAnimator.SetBool("IsAttacking", true);
        audioManager.PlayAttackHit();
        yield return StartCoroutine(MoveToPosition(playerRb, playerDeathBlowLocation2.position));
    }

    private IEnumerator JumpBackForDeathBlow()
    {
        playerMovement.Stop();
        playerAnimator.SetBool("IsCrouching", true);
        yield return new WaitForSeconds(2f);
        playerGhostTrail.StartTrail();
        audioManager.PlayArcadeJump();
        playerRb.AddForce( new Vector2(10f * Vector2.left.x, 30f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.25f);
        beginCheckingForGrounded = true;
    }

    private IEnumerator MoveToPosition(Rigidbody2D rb, Vector3 target)
    {
        float t = 0;
        Vector3 start = rb.position;

        while (t <= 1)
        {
            t += Time.fixedDeltaTime * 0.2f;
            rb.MovePosition(Vector3.Lerp(start, target, t));

            yield return null;
        }
    }
}
