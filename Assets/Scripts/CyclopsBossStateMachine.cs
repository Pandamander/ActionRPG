using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsBossStateMachine : MonoBehaviour
{
    [SerializeField] private Cyclops cyclops;
    private BoxCollider2D currentTarget;
    private float attackTimer = 1f;
    private float attackTimeCounter = 0f;
    private bool hasStarted = false;
    private float moveSpeed = 2.5f;
    private float moveDirection = -1f;

    public void Run()
    {
        cyclops.Walk();
        hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted) { return; }

        attackTimeCounter += Time.deltaTime;
        if (attackTimeCounter >= attackTimer)
        {
            Debug.Log("ATTACK!");
            attackTimeCounter = 0f;
            attackTimer = Random.Range(1.0f, 2.0f);
            cyclops.Attack(cyclops.attackTypes[Random.Range(0, cyclops.attackTypes.Length - 1)]);
        }

        cyclops.Move(moveSpeed * moveDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D: " + collision.collider.tag);
    }
}
