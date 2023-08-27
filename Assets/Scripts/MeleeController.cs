using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    public enum PlayerDirection { Left, Right };
    public MeleeWeapon currentMeleeWeapon;
    public PlayerDirection playerDirection;
    private Vector2 attackOriginPoint;
    private Vector2 attackSize;

    private void Awake()
    {
        attackSize = currentMeleeWeapon.attackBounds;
    }

    private void Update()
    {
        attackOriginPoint = new Vector2(
            transform.position.x,
            transform.position.y
        );

        if ( playerDirection == PlayerDirection.Left )
        {
            attackOriginPoint += new Vector2(-currentMeleeWeapon.attackPoint.x, currentMeleeWeapon.attackPoint.y);
        } else
        {
            attackOriginPoint += currentMeleeWeapon.attackPoint;
        }
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            attackOriginPoint,
            attackSize,
            transform.eulerAngles.z,
            currentMeleeWeapon.layerMask
        );

        DebugDrawBox(attackOriginPoint, attackSize);

        foreach (Collider2D c in hitEnemies)
        {
            if (c.gameObject.TryGetComponent<SubzoneEnemy>(out var enemy))
            {
                enemy.Damage(currentMeleeWeapon.attackDamage);
            }
        }
    }

    private void DebugDrawBox(Vector2 point, Vector2 size)
    {
        Vector2 bottomLeft = new Vector2(point.x - size.x / 2, point.y - size.y / 2);
        Vector2 bottomRight = new Vector2(point.x + size.x / 2, point.y - size.y / 2);
        Vector2 topRight = point + size / 2;
        Vector2 topLeft = new Vector2(point.x - size.x / 2, point.y + size.y / 2);

        Debug.DrawLine(bottomLeft, bottomRight, Color.red, 1f);
        Debug.DrawLine(bottomLeft, topLeft, Color.red, 1f);
        Debug.DrawLine(topLeft, topRight, Color.red, 1f);
        Debug.DrawLine(topRight, bottomRight, Color.red, 1f);
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
