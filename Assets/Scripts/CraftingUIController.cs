using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIController : MonoBehaviour
{
    public static bool IsOpen { get; private set; }
    public static int LastClosedFrame { get; private set; } = -1;

    [SerializeField] private CraftingRecipeCatalog recipeCatalog;
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text hammerLevelText;
    [SerializeField] private Transform recipeListContainer;
    [SerializeField] private CraftingUIRecipeRow recipeRowPrefab;
    [SerializeField] private CraftingUIRecipeRow doneRow;
    [SerializeField] private Color doneNormalColor = Color.white;
    [SerializeField] private Color doneSelectedColor = Color.yellow;

    private readonly List<CraftingUIRecipeRow> recipeRows = new List<CraftingUIRecipeRow>();
    private readonly List<CraftingRecipe> activeRecipes = new List<CraftingRecipe>();
    private PlayerMovement playerMovement;
    private int selectedIndex;
    private float axisInputDelayDuration = 0.25f;
    private bool acceptingAxisInputUp = true;
    private bool acceptingAxisInputDown = true;
    private Coroutine delayDownCoroutine;
    private Coroutine delayUpCoroutine;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        CloseImmediate();
    }

    public void Open()
    {
        PlayerStats.Initialize();
        PopulateRecipes();
        selectedIndex = 0;
        // Ignore held Up from opening the bench until the axis is released.
        acceptingAxisInputUp = false;
        acceptingAxisInputDown = false;
        hammerLevelText.text = $"LVL{PlayerStats.CraftingHammerLevel}";
        panelRoot.SetActive(true);
        IsOpen = true;

        if (playerMovement != null)
        {
            playerMovement.StopForDialogue();
        }

        RefreshSelection();
    }

    public void Close()
    {
        panelRoot.SetActive(false);
        LastClosedFrame = Time.frameCount;
        IsOpen = false;

        if (playerMovement != null)
        {
            playerMovement.canMove = true;
        }
    }

    private void CloseImmediate()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }

        IsOpen = false;
    }

    private void Update()
    {
        if (!IsOpen) return;

        HandleNavigation();
        HandleConfirm();
    }

    private void PopulateRecipes()
    {
        foreach (CraftingUIRecipeRow row in recipeRows)
        {
            if (row != null)
            {
                Destroy(row.gameObject);
            }
        }

        recipeRows.Clear();
        activeRecipes.Clear();

        List<CraftingRecipe> recipes = recipeCatalog.GetRecipesForHammerLevel(PlayerStats.CraftingHammerLevel);
        foreach (CraftingRecipe recipe in recipes)
        {
            CraftingUIRecipeRow row = Instantiate(recipeRowPrefab, recipeListContainer);
            row.BindRecipe(recipe);
            recipeRows.Add(row);
            activeRecipes.Add(recipe);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(recipeListContainer as RectTransform);
    }

    private void HandleNavigation()
    {
        float inputVertical = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("DPadY");
        int itemCount = recipeRows.Count + 1;

        if (inputVertical < 0 && acceptingAxisInputDown)
        {
            acceptingAxisInputDown = false;
            acceptingAxisInputUp = true;
            if (delayDownCoroutine != null)
            {
                StopCoroutine(delayDownCoroutine);
            }
            delayDownCoroutine = StartCoroutine(DelayAxisInputDown());
            selectedIndex = (selectedIndex + 1) % itemCount;
            RefreshSelection();
        }
        else if (inputVertical > 0 && acceptingAxisInputUp)
        {
            acceptingAxisInputUp = false;
            acceptingAxisInputDown = true;
            if (delayUpCoroutine != null)
            {
                StopCoroutine(delayUpCoroutine);
            }
            delayUpCoroutine = StartCoroutine(DelayAxisInputUp());
            selectedIndex = (selectedIndex - 1 + itemCount) % itemCount;
            RefreshSelection();
        }

        if (inputVertical == 0)
        {
            acceptingAxisInputDown = true;
            acceptingAxisInputUp = true;
        }
    }

    private void HandleConfirm()
    {
        if (!Input.GetButtonDown("Fire1") && !Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
        {
            return;
        }

        if (IsDoneSelected())
        {
            Close();
            return;
        }

        CraftingRecipe recipe = activeRecipes[selectedIndex];
        if (!recipe.CanCraft())
        {
            return;
        }

        if (recipe.Execute())
        {
            RefreshSelection();
        }
    }

    private void RefreshSelection()
    {
        for (int i = 0; i < recipeRows.Count; i++)
        {
            recipeRows[i].RefreshState(activeRecipes[i], i == selectedIndex);
        }

        bool doneSelected = IsDoneSelected();
        doneRow.SetDoneRow(doneSelected);
        if (doneRow.TitleText != null)
        {
            doneRow.TitleText.color = doneSelected ? doneSelectedColor : doneNormalColor;
        }
    }

    private bool IsDoneSelected()
    {
        return selectedIndex >= recipeRows.Count;
    }

    private IEnumerator DelayAxisInputDown()
    {
        float elapsedTime = 0f;
        while (elapsedTime < axisInputDelayDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        acceptingAxisInputDown = true;
        delayDownCoroutine = null;
    }

    private IEnumerator DelayAxisInputUp()
    {
        float elapsedTime = 0f;
        while (elapsedTime < axisInputDelayDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        acceptingAxisInputUp = true;
        delayUpCoroutine = null;
    }
}
