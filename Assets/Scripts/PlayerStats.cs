using System.Collections.Generic;
using UnityEngine;
public static class PlayerStats
{
    public static int Attack { get; private set; }
    public static int Defense { get; private set; }
    public static int DefenseCapacity { get; private set; }
    public static int Health { get; private set; }
    public static int HealthCapacity { get; private set; }
    public static string MeleeWeapon { get; private set; }
    public static string SecondaryWeapon { get; private set; }
    public static List<string> SecondaryWeapons { get; private set; }

    private static Dictionary<string, int> SecondaryWeaponAmmo;

    public static List<string> PowerupDestroy { get; private set; }

    public static List<string> OverworldDestroyList { get; private set; }

    private static bool Initialized;

    public static void Initialize()
    {
        if (!Initialized)
        {
            Attack = 0;
            DefenseCapacity = 1;
            Defense = DefenseCapacity;
            HealthCapacity = 14;
            Health = HealthCapacity;
            Initialized = true;
            PowerupDestroy = new List<string>();
            OverworldDestroyList = new List<string>();
            MeleeWeapon = "GladiusSword";
            SecondaryWeapons = new List<string>();
            SecondaryWeapons.Add("ThrowingAxe");
            SecondaryWeapons.Add("Plumbata");
            SecondaryWeapon = "ThrowingAxe";
            SecondaryWeaponAmmo = new Dictionary<string, int>();
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

    public static void PickUpWeapon(string weaponSOPath, int attack)
    {
        Debug.Log("PickUpWeapon: " + weaponSOPath);
        MeleeWeapon = weaponSOPath;
        Attack = attack;
    }

    public static void AcquireSecondaryWeapon(string weaponName)
    {
        Debug.Log("AcquireSecondaryWeapon: " + weaponName);
        if (!SecondaryWeapons.Contains(weaponName))
        {
            SecondaryWeapons.Add(weaponName);
        }

        EquipSecondaryWeapon(weaponName);
    }

    public static void EquipSecondaryWeapon(string weaponName)
    {
        Debug.Log("EquipSecondaryWeapon: " + weaponName);
        SecondaryWeapon = weaponName;
    }

    public static int GetSecondaryWeaponAmmo(string weaponName)
    {
        return SecondaryWeaponAmmo != null && SecondaryWeaponAmmo.TryGetValue(weaponName, out int ammo) ? ammo : 0;
    }

    public static void InitializeSecondaryWeaponAmmo(string weaponName, int maxAmmo)
    {
        if (SecondaryWeaponAmmo == null)
        {
            SecondaryWeaponAmmo = new Dictionary<string, int>();
        }

        if (!SecondaryWeaponAmmo.ContainsKey(weaponName))
        {
            SecondaryWeaponAmmo[weaponName] = maxAmmo;
        }
    }

    public static void ConsumeSecondaryWeaponAmmo(string weaponName)
    {
        if (SecondaryWeaponAmmo == null || !SecondaryWeaponAmmo.ContainsKey(weaponName))
        {
            return;
        }

        SecondaryWeaponAmmo[weaponName] = Mathf.Max(0, SecondaryWeaponAmmo[weaponName] - 1);
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
