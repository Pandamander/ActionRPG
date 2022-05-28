public static class PlayerStats
{
    public static int Attack { get; private set; }
    public static int Defense { get; private set; }
    public static float Health { get; private set; }
    public static float MaxHealth { get; private set; }

    private static string[] WeaponVariation = { "" };

    public static void InitializeStats()
    {
        Attack = 4;
        Defense = 4;
        MaxHealth = 4;
        Health = MaxHealth;
    }

    public static void UpgradetAttack()
    {
        Attack += 1;
    }

    public static void UpgradeDefense()
    {
        Defense += 1;
    }

    public static void UpgradeHealth()
    {
        MaxHealth += 1;
    }

    public static float ApplyDamage(float amount)
    {
        Health -= amount;
        return Health;
    }

    public static string WeaponVariationName()
    {
        return WeaponVariation[Attack];
    }
}
