using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public enum BossState { Idle, Intro, Sin, Swoop }
    private BossState currentState = BossState.Intro;
    private float sinMovmentSpeed;
    private float swoopAttackMovementSpeed;
    private float sinFrequency = 4f;
    private float sinMagnitude = 4f;
    private Rigidbody2D rigidBody;
    private Vector3 start;
    private float sinHorizontalMovementTimer = 0f;
    private float sinHorizontalMovementFlip = 1f;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        start = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3(48.0400009,4.5999999,0)

        switch (currentState)
        {
            case BossState.Idle:
                break;
            case BossState.Intro:
                rigidBody.position = start + transform.up * Mathf.Sin(Time.time * 1f) * 1f;
                break;
            case BossState.Sin:
                if (sinHorizontalMovementFlip > 0)
                {
                    sinHorizontalMovementTimer += Time.deltaTime + 0.005f;
                }
                else
                {
                    sinHorizontalMovementTimer -= (Time.deltaTime + 0.005f);
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
                break;
        }
    }

    public void TransitionTo(BossState state)
    {
        StartCoroutine(MoveState(state));
    }

    public void UpdateMovementSpeeds(float sinMovmentSpeed, float swoopAttackMovementSpeed)
    {
        this.sinMovmentSpeed = sinMovmentSpeed;
        this.swoopAttackMovementSpeed = swoopAttackMovementSpeed;
    }

    private IEnumerator MoveState(BossState state)
    {
        StartCoroutine(Flash());
        yield return new WaitForSeconds(2f);
        currentState = state;
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = Color.cyan;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.cyan;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.cyan;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
    }
}
