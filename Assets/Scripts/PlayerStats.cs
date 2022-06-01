using System.Collections.Generic;
public static class PlayerStats
{
    public static int Attack { get; private set; }
    public static int Defense { get; private set; }
    public static int DefenseCapacity { get; private set; }
    public static float Health { get; private set; }
    public static float HealthCapacity { get; private set; }

    public static List<string> PowerupDestroy { get; private set; }
    

    private static bool Initialized;    

    public static void Initialize()
    {
        if (!Initialized)
        {
            Attack = 1;
            DefenseCapacity = 1;
            Defense = DefenseCapacity;
            HealthCapacity = 1;
            Health = HealthCapacity;
            Initialized = true;
            PowerupDestroy = new List<string>();
        }
    }

    public static void UpgradetAttack(string tag)
    {
        Attack += 1;
        PowerupDestroy.Add(tag);
    }

    public static void UpgradeDefense(string tag)
    {
        DefenseCapacity += 1;
        Defense = DefenseCapacity;
        PowerupDestroy.Add(tag);
    }

    public static void UpgradeHealth(string tag)
    {
        HealthCapacity += 1;
        Health = HealthCapacity;
        PowerupDestroy.Add(tag);
    }

    public static void ApplyDamage(float amount)
    {
        if (Defense == 0)
        {
            float newHealth = Health - amount;
            if (newHealth < 0f)
            {
                newHealth = 0f;
            }
            Health = newHealth;
        }
        else
        {
            Defense -= 1;
        }
    }

    public static void Reset()
    {
        Initialized = false;
    }
}
