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
    [SerializeField] private GameObject costGroup;
    [SerializeField] private Sprite unselectedBackgroundSprite;
    [SerializeField] private Sprite selectedBackgroundSprite;
    [SerializeField] private Color normalCostColor = Color.white;
    [SerializeField] private Color insufficientCostColor = Color.red;
    [SerializeField] private Color unavailableColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    public RectTransform RectTransform => transform as RectTransform;
    public TMP_Text TitleText => titleText;

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

    public void SetDoneRow(bool isSelected)
    {
        if (costGroup != null)
        {
            costGroup.SetActive(false);
        }

        if (costText != null)
        {
            costText.gameObject.SetActive(false);
        }

        if (iconImage != null)
        {
            iconImage.enabled = false;
        }

        SetBackgroundSelected(isSelected);
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
            oreIconImage.enabled = false;
            costText.gameObject.SetActive(true);
            costText.text = recipe.unavailableLabel;
            costText.color = unavailableColor;
            return;
        }

        costGroup.SetActive(true);
        oreIconImage.enabled = true;
        costText.gameObject.SetActive(true);
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
