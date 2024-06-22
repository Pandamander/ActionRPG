using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsBossStateMachine : MonoBehaviour
{
    public enum BossState { NotStarted, Attacking, Dying, Dead }

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
    private float swipeDistance = 3f;
    private bool startedDeathSequence = false;
    public void Run()
    {
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
                    bossState = BossState.Dying;
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

                if (Mathf.Abs(cyclops.transform.position.x - playerRb.position.x) < swipeDistance)
                {
                    cyclops.Attack(Cyclops.AttackType.Swipe);
                }

                break;
            case BossState.Dying:
                if (startedDeathSequence) return;
                startedDeathSequence = true;
                audioManager.StopMusic();
                StartCoroutine(CyclopsDie());
                break;
        }
    }

    private IEnumerator CyclopsDie()
    {
        playerMovement.Stop(overrideAttack: true);
        cyclops.Die();
        yield return new WaitForSeconds(10.0f);
        Destroy(cyclops.gameObject);
        bossState = BossState.Dead;
        playerMovement.AllowMovement();
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
