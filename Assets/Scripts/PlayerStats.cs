public static class PlayerStats
{
    public static int Attack { get; private set; }
    public static int Defense { get; private set; }
    public static int DefenseCapacity { get; private set; }
    public static float Health { get; private set; }
    public static float HealthCapacity { get; private set; }

    private static bool Initialized;
    private static string[] WeaponVariation = { "" };
    

    public static void Initialize()
    {
        if (!Initialized)
        {
            Attack = 10;
            DefenseCapacity = 1;
            Defense = DefenseCapacity;
            HealthCapacity = 1;
            Health = HealthCapacity;
            Initialized = true;
        }
    }

    public static void UpgradetAttack()
    {
        Attack += 1;
    }

    public static void UpgradeDefense()
    {
        DefenseCapacity += 1;
        Defense = DefenseCapacity;
    }

    public static void UpgradeHealth()
    {
        HealthCapacity += 1;
        Health = HealthCapacity;
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

    public static string WeaponVariationName()
    {
        return WeaponVariation[Attack];
    }

    public static void Reset()
    {
        Initialized = false;
    }
}
