using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "ScriptableObjects/Weapon/Melee")]
public class MeleeWeapon : ScriptableObject
{
    public Vector2 attackPoint;
    public Vector2 attackBounds;
    public float attackDamage;
    public LayerMask attackLayerMask;
}