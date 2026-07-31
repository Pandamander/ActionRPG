using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIRecipeRow : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image oreIconImage;
    [SerializeField] private Image lockIconImage;
    [SerializeField] private GameObject costGroup;
    [SerializeField] private Sprite unselectedBackgroundSprite;
    [SerializeField] private Sprite selectedBackgroundSprite;
    [SerializeField] private Color normalCostColor = Color.white;
    [SerializeField] private Color insufficientCostColor = Color.red;
    [SerializeField] private Color unavailableColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    public RectTransform RectTransform => transform as RectTransform;

    public void BindRecipe(CraftingRecipe recipe)
    {
        RefreshState(recipe, false);
    }

    public void RefreshState(CraftingRecipe recipe, bool isSelected)
    {
        SetBackgroundSelected(isSelected);

        if (recipe == null)
        {
            return;
        }

        if (!recipe.MeetsRequirements())
        {
            titleText.text = !string.IsNullOrEmpty(recipe.lockedDisplayName)
                ? recipe.lockedDisplayName
                : recipe.displayName;
            titleText.color = unavailableColor;
            SetItemIcon(recipe.lockedIcon != null ? recipe.lockedIcon : recipe.icon);
            costGroup.SetActive(true);
            oreIconImage.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            lockIconImage.gameObject.SetActive(true);
            return;
        }

        titleText.text = recipe.displayName;
        titleText.color = Color.white;
        SetItemIcon(recipe.icon);
        costGroup.SetActive(true);
        oreIconImage.gameObject.SetActive(true);
        costText.gameObject.SetActive(true);
        lockIconImage.gameObject.SetActive(false);
        costText.text = $"{recipe.oreCost:D2}";

        if (recipe.IsAtMaxAmmo())
        {
            costText.color = unavailableColor;
        }
        else
        {
            costText.color = recipe.CanAfford() ? normalCostColor : insufficientCostColor;
        }
    }

    private void SetItemIcon(Sprite sprite)
    {
        iconImage.enabled = sprite != null;
        if (sprite != null)
        {
            iconImage.sprite = sprite;
        }
    }

    private void SetBackgroundSelected(bool isSelected)
    {
        if (backgroundImage == null)
        {
            return;
        }

        Sprite sprite = isSelected ? selectedBackgroundSprite : unselectedBackgroundSprite;
        if (sprite != null)
        {
            backgroundImage.sprite = sprite;
        }
    }
}
