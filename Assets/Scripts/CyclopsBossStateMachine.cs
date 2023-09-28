using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsBossStateMachine : MonoBehaviour
{
    [SerializeField] private Cyclops cyclops;
    private BoxCollider2D currentTarget;
    private float attackTimer = 2f;
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
            attackTimeCounter = 0f;
            attackTimer = Random.Range(4.0f, 8.0f);
            //int randAttack = Random.Range(0, cyclops.attackTypes.Length - 1);
            //Debug.Log("randAttack: " + randAttack);
            cyclops.Attack(cyclops.attackTypes[0]);
        }

        cyclops.Move(moveSpeed * moveDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D: " + collision.collider.tag);
    }
}
