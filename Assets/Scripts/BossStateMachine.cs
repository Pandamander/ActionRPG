using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossStateMachine : MonoBehaviour
{
    public enum BossState { Idle, Intro, Sin, Swoop, Die }
    private BossState currentState = BossState.Intro;
    public BossState currentlyRunningState
    {
        get
        {
            return currentState;
        }
    }
    public Transform swoopLeft;
    public Transform swoopRight;
    private float sinMovmentSpeed;
    private float swoopAttackMovementSpeed;
    private float sinFrequency = 4f;
    private float sinMagnitude = 4f;
    private Rigidbody2D rigidBody;
    private Vector3 start;
    private float sinHorizontalMovementTimer = 0f;
    private float sinHorizontalMovementFlip = 1f;
    private SpriteRenderer spriteRenderer;
    private BossFightManager bossFightManager;
    private Vector3 moveTo;
    private BossCombat combat;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        bossFightManager = GetComponent<BossFightManager>();
        combat = GetComponent<BossCombat>();
        start = transform.position;
        moveTo = swoopRight.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3(48.0400009,4.5999999,0)

        switch (currentState)
        {
            case BossState.Die:
                rigidBody.velocity = new Vector2(0f, -80 * Time.fixedDeltaTime);
                transform.Rotate(0f, 0f, 4 * 360 * Time.deltaTime);
                if (transform.position.y <= -6)
                {
                    SceneManager.LoadScene("Victory");
                }
                break;
            case BossState.Idle:
                break;
            case BossState.Intro:
                rigidBody.position = start + transform.up * Mathf.Sin(Time.time * 1f) * 1f;
                break;
            case BossState.Sin:
                if (sinHorizontalMovementFlip > 0)
                {
                    sinHorizontalMovementTimer += (Time.deltaTime + 0.005f) * sinMovmentSpeed;
                }
                else
                {
                    sinHorizontalMovementTimer -= (Time.deltaTime + 0.005f) * sinMovmentSpeed;
                }
                
                if (sinHorizontalMovementTimer >= 10f || sinHorizontalMovementTimer <= -10f)
                {
                    sinHorizontalMovementFlip *= -1;
                }
                Vector3 pos = start;
                pos.x += sinHorizontalMovementTimer;
                rigidBody.position = pos + transform.up * Mathf.Sin(Time.time * sinFrequency) * sinMagnitude;

                break;
            case BossState.Swoop:
                rigidBody.transform.position = Vector3.MoveTowards(transform.position, moveTo, swoopAttackMovementSpeed * Time.deltaTime);

                float distanceToMovePoint = Vector3.Distance(transform.position, moveTo);
                if (distanceToMovePoint == 0)
                {
                    moveTo = moveTo == swoopLeft.position ? swoopRight.position : swoopLeft.position;
                }
                
                break;
        }
    }

    public void TransitionTo(BossState state)
    {
        StartCoroutine(MoveState(state));
    }

    public void Run()
    {
        StartCoroutine(MoveToStartingState(BossState.Sin));
    }

    public void UpdateMovementSpeeds(float sinMovmentSpeed, float swoopAttackMovementSpeed)
    {
        this.sinMovmentSpeed = sinMovmentSpeed;
        this.swoopAttackMovementSpeed = swoopAttackMovementSpeed;
    }

    public void BossDead()
    {
        StartCoroutine(DeathFlash(Color.red));
        currentState = BossState.Die;
    }

    private IEnumerator MoveToStartingState(BossState state)
    {
        StartCoroutine(Flash(Color.cyan));
        yield return new WaitForSeconds(3f);
        currentState = state;
        bossFightManager.InitialBossPhaseDidBegin();
        combat.InitialBossPhaseDidBegin();
    }

    private IEnumerator MoveState(BossState state)
    {
        StartCoroutine(Flash(Color.green));
        yield return new WaitForSeconds(3f);
        currentState = state;
        sinHorizontalMovementTimer = 0f;
    }

    private IEnumerator Flash(Color color)
    {
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator DeathFlash(Color color)
    {
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
    }
}
