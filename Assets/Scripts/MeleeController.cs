using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    public MeleeWeapon currentMeleeWeapon;

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            currentMeleeWeapon.attackPoint,
            currentMeleeWeapon.attackBounds,
            transform.eulerAngles.z,
            currentMeleeWeapon.attackLayerMask
        );

        foreach(Collider2D c in hitEnemies)
        {
            Debug.Log("HIT: " + c.name);
            IDamageable enemy = c as IDamageable;
            if (enemy != null)
            {
                Debug.Log("ENEMY DAMAGE");
                enemy.Damage(currentMeleeWeapon.attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (currentMeleeWeapon == null) return;
        Gizmos.DrawWireCube(
            currentMeleeWeapon.attackPoint,
            currentMeleeWeapon.attackBounds
        );
    }
}