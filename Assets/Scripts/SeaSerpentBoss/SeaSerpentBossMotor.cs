using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentBossMotor : MonoBehaviour
{
    [Header("Idle Figure 8")]
    [SerializeField] private float idleWidth = 0.5f;
    [SerializeField] private float idleHeight = 0.25f;
    [SerializeField] private float idleSpeed = 1.5f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;

    private Vector3 basePosition;
    private Vector3 moveTarget;
    private bool idleActive;
    private bool movingToTarget;
    private float idleTime;

    private void Awake()
    {
        basePosition = transform.position;
        moveTarget = transform.position;
    }

    public void SetBasePosition(Vector3 position)
    {
        basePosition = position;
        moveTarget = position;
    }

    public Vector3 GetBasePosition()
    {
        return basePosition;
    }

    public void StartIdle()
    {
        idleActive = true;
        movingToTarget = false;
        idleTime = 0f;
        basePosition = transform.position;
        Debug.Log(basePosition);
        Debug.Log(this);
    }

    public void StopIdle()
    {
        idleActive = false;
    }

    public void MoveTo(Vector3 target)
    {
        idleActive = false;
        movingToTarget = true;
        moveTarget = target;
    }

    public bool IsAtTarget(float threshold = 0.05f)
    {
        return Vector3.Distance(transform.position, moveTarget) <= threshold;
    }

    public void Tick()
    {
        if (movingToTarget)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                moveTarget,
                moveSpeed * Time.deltaTime
            );

            if (IsAtTarget())
            {
                transform.position = moveTarget;
                basePosition = moveTarget;
                movingToTarget = false;
            }

            return;
        }

        if (idleActive)
        {
            idleTime += Time.deltaTime * idleSpeed;

            float x = Mathf.Sin(idleTime) * idleWidth;
            float y = Mathf.Sin(idleTime * 2f) * idleHeight;

            transform.position = basePosition + new Vector3(x, y, 0f);
        }
    }
}
