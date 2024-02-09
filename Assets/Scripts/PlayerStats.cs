using System.Collections.Generic;
using UnityEngine;
public static class PlayerStats
{
    public static int Attack { get; private set; }
    public static int Defense { get; private set; }
    public static int DefenseCapacity { get; private set; }
    public static int Health { get; private set; }
    public static int HealthCapacity { get; private set; }

    public static List<string> PowerupDestroy { get; private set; }

    public static List<string> OverworldDestroyList { get; private set; }

    private static bool Initialized;

    public static void Initialize()
    {
        if (!Initialized)
        {
            Attack = 1;
            DefenseCapacity = 1;
            Defense = DefenseCapacity;
            HealthCapacity = 14;
            Health = HealthCapacity;
            Initialized = true;
            PowerupDestroy = new List<string>();
            OverworldDestroyList = new List<string>();
        }
    }

    public static void BossDefeated(string tag)
    {
        OverworldDestroyList.Add(tag);
    }

    public static void UpgradetAttack(string tag)
    {
        Attack += 1;
        PowerupDestroy.Add(tag);
        Debug.Log("ADDED TAG: " + tag);
        Debug.Log("PowerupDestroy: " + PowerupDestroy.Count);
    }

    public static void UpgradeDefense(string tag)
    {
        DefenseCapacity += 1;
        Defense = DefenseCapacity;
        PowerupDestroy.Add(tag);
        Debug.Log("ADDED TAG: " + tag);
        Debug.Log("PowerupDestroy: " + PowerupDestroy.Count);
    }

    public static void UpgradeHealth(string tag)
    {
        HealthCapacity += 1;
        Health = HealthCapacity;
        PowerupDestroy.Add(tag);
        Debug.Log("ADDED TAG: " + tag);
        Debug.Log("PowerupDestroy: " + PowerupDestroy.Count);
    }

    public static void ApplyDamage(int amount)
    {
        int newHealth = Health - amount;
        if (newHealth < 0)
        {
            newHealth = 0;
        }
        Health = newHealth;
    }

    public static void Reset()
    {
        Initialized = false;
    }

    public static void ResetHealthForContinue()
    {
        Health = HealthCapacity;
    }
}
