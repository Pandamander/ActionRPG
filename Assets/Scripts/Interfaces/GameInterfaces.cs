using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(int damage, float damageDirection);
}

public interface IMineable
{
    void Mine(float damageDirection);
}

public interface IBreakable
{
    void Hit(float damageDirection);
}
