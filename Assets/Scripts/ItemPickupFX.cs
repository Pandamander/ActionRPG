using UnityEngine;

public class ItemPickupFX : MonoBehaviour
{
    public void AnimationComplete()
    {
        Destroy(gameObject);
    }
}
