using System;
using System.Collections.Generic;
using UnityEngine;

public enum CraftRequirementType
{
    SecondaryWeaponAcquired
}

[Serializable]
public class CraftRequirement
{
    public CraftRequirementType type;
    public SecondaryWeapon secondaryWeapon;
}

public enum CraftEffectType
{
    AddSecondaryWeaponAmmo
}

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "ScriptableObjects/Crafting/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string displayName;
    public string lockedDisplayName;
    public Sprite icon;
    public Sprite lockedIcon;
    public int hammerLevelRequired = 1;
    public int oreCost;
    public string unavailableLabel = "UNAVAILABLE";
    public List<CraftRequirement> requirements = new List<CraftRequirement>();
    public CraftEffectType effectType = CraftEffectType.AddSecondaryWeaponAmmo;
    public SecondaryWeapon targetSecondaryWeapon;
    public int ammoAmount;

    public bool MeetsRequirements()
    {
        PlayerStats.Initialize();

        foreach (CraftRequirement requirement in requirements)
        {
            if (!EvaluateRequirement(requirement))
            {
                return false;
            }
        }

        return true;
    }

    public bool CanAfford()
    {
        PlayerStats.Initialize();
        return PlayerStats.Ore >= oreCost;
    }

    public bool IsAtMaxAmmo()
    {
        if (effectType != CraftEffectType.AddSecondaryWeaponAmmo || targetSecondaryWeapon == null)
        {
            return false;
        }

        PlayerStats.Initialize();
        return PlayerStats.GetSecondaryWeaponAmmo(targetSecondaryWeapon.name) >= targetSecondaryWeapon.maxAmmo;
    }

    public bool CanCraft()
    {
        return MeetsRequirements() && CanAfford() && !IsAtMaxAmmo();
    }

    public bool Execute()
    {
        if (!CanCraft())
        {
            return false;
        }

        if (!PlayerStats.SpendOre(oreCost))
        {
            return false;
        }

        switch (effectType)
        {
            case CraftEffectType.AddSecondaryWeaponAmmo:
                if (targetSecondaryWeapon == null)
                {
                    return false;
                }

                PlayerStats.AddSecondaryWeaponAmmo(
                    targetSecondaryWeapon.name,
                    ammoAmount,
                    targetSecondaryWeapon.maxAmmo
                );
                break;
        }

        return true;
    }

    private static bool EvaluateRequirement(CraftRequirement requirement)
    {
        switch (requirement.type)
        {
            case CraftRequirementType.SecondaryWeaponAcquired:
                return requirement.secondaryWeapon != null
                    && PlayerStats.SecondaryWeapons.Contains(requirement.secondaryWeapon.name);
            default:
                return true;
        }
    }
}
