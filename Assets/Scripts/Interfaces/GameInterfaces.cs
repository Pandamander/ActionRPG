using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(int damage, float damageDirection);
}

public interface IMeleeDamageable
{
    void DamageFromMelee(float damageDirection);
}
