using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIDoneButton : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private Sprite unselectedBackgroundSprite;
    [SerializeField] private Sprite selectedBackgroundSprite;
    [SerializeField] private Color normalLabelColor = Color.white;
    [SerializeField] private Color selectedLabelColor = Color.yellow;

    public void SetSelected(bool isSelected)
    {
        Sprite sprite = isSelected ? selectedBackgroundSprite : unselectedBackgroundSprite;
        if (backgroundImage != null && sprite != null)
        {
            backgroundImage.sprite = sprite;
        }

        if (labelText != null)
        {
            labelText.color = isSelected ? selectedLabelColor : normalLabelColor;
        }
    }
}
