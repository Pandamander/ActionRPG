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

    private void Awake()
    {
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
        switch (currentState)
        {
            case BossState.Idle:
                break;
            case BossState.Intro:
                rigidBody.position = start + transform.up * Mathf.Sin(Time.time * 1f) * 1f;
                break;
            case BossState.Sin:
                rigidBody.position = start + transform.up * Mathf.Sin(Time.time * sinFrequency) * sinMagnitude;
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
        yield return new WaitForSeconds(2f);
        currentState = state;
    }
}
