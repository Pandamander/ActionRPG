using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentBossAttackController : MonoBehaviour
{
    [SerializeField] private float attackDuration = 1.0f;

    private float attackTimer;
    private bool attacking;

    public void StartAttack(string attackName)
    {
        attacking = true;
        attackTimer = attackDuration;

        Debug.Log($"Boss started attack: {attackName}");
    }

    public void Tick()
    {
        if (!attacking) return;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attacking = false;
        }
    }

    public bool IsAttackFinished()
    {
        return !attacking;
    }
}
