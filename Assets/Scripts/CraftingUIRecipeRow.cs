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
        iconImage.enabled = recipe.icon != null;
        if (recipe.icon != null)
        {
            iconImage.sprite = recipe.icon;
        }

        titleText.text = recipe.displayName;
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
            costGroup.SetActive(true);
            oreIconImage.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            lockIconImage.gameObject.SetActive(true);
            return;
        }

        costGroup.SetActive(true);
        oreIconImage.gameObject.SetActive(true);
        costText.gameObject.SetActive(true);
        lockIconImage.gameObject.SetActive(false);
        costText.text = $"x {recipe.oreCost:D2}";

        if (recipe.IsAtMaxAmmo())
        {
            costText.color = unavailableColor;
        }
        else
        {
            costText.color = recipe.CanAfford() ? normalCostColor : insufficientCostColor;
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
