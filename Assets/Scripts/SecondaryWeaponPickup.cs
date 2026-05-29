using UnityEngine;

public class SecondaryWeaponPickup : MonoBehaviour
{
    public SecondaryWeapon weapon;

    private void Awake()
    {
        PlayerStats.Initialize();

        if (weapon != null && PlayerStats.SecondaryWeapons.Contains(weapon.name))
        {
            transform.root.gameObject.SetActive(false);
        }
    }
}
