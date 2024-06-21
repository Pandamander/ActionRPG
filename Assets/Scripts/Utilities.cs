using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float DamageDirection(GameObject damager, GameObject damagee)
    {
        return (damagee.transform.position.x - damager.transform.position.x < 0f) ? -1f : 1f;
    }
}
