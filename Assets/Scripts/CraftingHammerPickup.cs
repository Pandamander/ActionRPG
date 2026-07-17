using UnityEngine;

public class CraftingHammerPickup : MonoBehaviour
{
    private void Awake()
    {
        PlayerStats.Initialize();

        if (PlayerStats.HasCraftingHammer)
        {
            transform.root.gameObject.SetActive(false);
        }
    }
}
