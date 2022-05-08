using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DynamicObstacleDamageType
{
    SpinNoSteering,
    TiresBlown,
    Slow
}

public interface IDynamicObstacle
{
    DynamicObstacleDamageType DamageType { get; }
    float DamageDuration { get; }
}

public class DynamicObstacle : MonoBehaviour, IDynamicObstacle
{
    // IDynamicObstacle

    // Damage Type
    public DynamicObstacleDamageType type;
    public DynamicObstacleDamageType DamageType
    {
        get
        {
            return type;
        }
    }

    // Damage Duration
    public float duration;
    public float DamageDuration
    {
        get
        {
            return duration;
        }
    }
}