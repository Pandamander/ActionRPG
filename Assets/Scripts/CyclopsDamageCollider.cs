using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsDamageCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.Damage(1, Utilities.DamageDirection(gameObject, collision.gameObject));
            }
        }
    }
}
