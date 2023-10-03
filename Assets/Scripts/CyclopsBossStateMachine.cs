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
    private float moveTimer = 8f;
    private float moveTimeCounter = 0f;
    private List<int> swipeHealths = new() { 11, 8, 5, 2 };
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
            attackTimer = Random.Range(2.0f, 3.0f);
            int randAttack = Random.Range(0, cyclops.autoAttackTypes.Length);
            cyclops.Attack(cyclops.autoAttackTypes[randAttack]);
            //cyclops.Attack(Cyclops.AttackType.Swipe);
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
    }
}
