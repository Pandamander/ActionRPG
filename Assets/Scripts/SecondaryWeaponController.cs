using System.Collections.Generic;
using UnityEngine;

public class SecondaryWeaponController : MonoBehaviour
{
    [SerializeField] private List<SecondaryWeapon> weapons;

    public SecondaryWeapon currentWeapon { get; private set; }
    public bool HasWeapon
    {
        get
        {
            return currentWeapon != null;
        }
    }
    public bool CanAttack => _cooldownTimer <= 0f;

    private Dictionary<string, SecondaryWeapon> _weaponMap;
    private float _cooldownTimer;

    private void Awake()
    {
        InitializeWeaponMap();
        LoadLastObtainedWeapon();

        SetWeapon(weapons[0]);
    }

    private void Update()
    {
        if (_cooldownTimer > 0f)
            _cooldownTimer -= Time.deltaTime;
    }

    public void Attack(Vector2 direction, bool isCrouching)
    {
        if (!HasWeapon || _cooldownTimer > 0f) return;
        currentWeapon.Execute(transform, direction, isCrouching);
        _cooldownTimer = currentWeapon.cooldown;
    }

    public void PickUpWeapon(SecondaryWeapon weapon)
    {
        PlayerStats.PickUpSecondaryWeapon(weapon.name);
        SetWeapon(weapon);
    }

    private void SetWeapon(SecondaryWeapon weapon)
    {
        currentWeapon = weapon;
        //FindObjectOfType<SubzoneHUD>().SetSecondaryItemFrameImage(weapon.itemFrameImage);
    }

    private void InitializeWeaponMap()
    {
        _weaponMap = new Dictionary<string, SecondaryWeapon>();
        foreach (SecondaryWeapon weapon in weapons)
        {
            _weaponMap[weapon.name] = weapon;
        }

    }

    private void LoadLastObtainedWeapon()
    {
        if (PlayerStats.SecondaryWeapon != null && _weaponMap.ContainsKey(PlayerStats.SecondaryWeapon))
        {
            SetWeapon(_weaponMap[PlayerStats.SecondaryWeapon]);
        }
    }
}
