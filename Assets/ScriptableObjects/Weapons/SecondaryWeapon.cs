using UnityEngine;

public abstract class SecondaryWeapon : ScriptableObject
{
    public Sprite itemFrameImage;
    public float cooldown = 0.5f;

    public abstract void Execute(Transform player, Vector2 direction);
}
