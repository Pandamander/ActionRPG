using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MeleeController : MonoBehaviour
{
    public enum PlayerDirection { Left, Right };
    public MeleeWeapon currentMeleeWeapon { get; private set; }
    [SerializeField] private SubzoneAudioManager audioManager;
    [SerializeField] private List<MeleeWeapon> weapons;
    private Dictionary<string, MeleeWeapon> WeaponScriptableObjectMap;
    public PlayerDirection playerDirection;
    public bool isCrouching = false;
    public bool HasWeapon
    {
        get
        {
            return currentMeleeWeapon != null;
        }
    }
    private Vector2 attackOriginPoint;
    private Vector2 attackSize;

    private void Awake()
    {
        InitializeWeaponMap();

        LoadLastObtainedWeapon();

        if (!HasWeapon) return;

        attackSize = currentMeleeWeapon.attackBounds;
    }

    private void LoadLastObtainedWeapon()
    {
        if (PlayerStats.MeleeWeapon != null)
        {
            MeleeWeapon weapon = WeaponScriptableObjectMap[PlayerStats.MeleeWeapon];
            if (weapon != null)
            {
                currentMeleeWeapon = weapon;
            }
        }
    }

    private void InitializeWeaponMap()
    {
        WeaponScriptableObjectMap = new Dictionary<string, MeleeWeapon>()
        {
            { "GladiusSword", weapons[0] },
        };
    }

    private void Update()
    {
        if (!HasWeapon) return;

        attackOriginPoint = new Vector2(
            transform.position.x,
            transform.position.y
        );

        Vector2 weaponAttackPoint = isCrouching ? currentMeleeWeapon.crouchAttackPoint : currentMeleeWeapon.attackPoint;
        if ( playerDirection == PlayerDirection.Left )
        {
            attackOriginPoint += new Vector2(-weaponAttackPoint.x, weaponAttackPoint.y);
        } else
        {
            attackOriginPoint += weaponAttackPoint;
        }
    }

    public void Attack()
    {
        if (!HasWeapon) return;

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            attackOriginPoint,
            attackSize,
            transform.eulerAngles.z,
            currentMeleeWeapon.layerMask
        );

        //Debug.Log("HIT " + hitEnemies.Length + " ENEMIES");

        if (hitEnemies.Length > 0)
        {
            audioManager.PlayAttackHit();
        } else
        {
            audioManager.PlayAttack();
        }

        DebugDrawBox(attackOriginPoint, attackSize);

        foreach (Collider2D c in hitEnemies)
        {
            if (c.gameObject.TryGetComponent<IDamageable>(out var enemy))
            {
                enemy.Damage(currentMeleeWeapon.attackDamage);
            }
        }
    }

    public void SetMeleeWeapon(MeleeWeapon weapon)
    {
        PlayerStats.PickUpWeapon(weapon.name);
        currentMeleeWeapon = weapon;
    }

    private void DebugDrawBox(Vector2 point, Vector2 size)
    {
        Vector2 bottomLeft = new Vector2(point.x - size.x / 2, point.y - size.y / 2);
        Vector2 bottomRight = new Vector2(point.x + size.x / 2, point.y - size.y / 2);
        Vector2 topRight = point + size / 2;
        Vector2 topLeft = new Vector2(point.x - size.x / 2, point.y + size.y / 2);

        Debug.DrawLine(bottomLeft, bottomRight, Color.red, 1f);
        Debug.DrawLine(bottomLeft, topLeft, Color.red, 1f);
        Debug.DrawLine(topLeft, topRight, Color.red, 1f);
        Debug.DrawLine(topRight, bottomRight, Color.red, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        if (!HasWeapon) return;

        Gizmos.DrawWireCube(
            currentMeleeWeapon.attackPoint,
            currentMeleeWeapon.attackBounds
        );
    }
}
