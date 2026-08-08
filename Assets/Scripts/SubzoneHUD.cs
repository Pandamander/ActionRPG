using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubzoneHUD : MonoBehaviour
{
    [SerializeField] private HUDHealthMeter playerHealthMeter;
    [SerializeField] private HUDHealthMeter bossHealthMeter;
    [SerializeField] private TMP_Text attackValueText;
    [SerializeField] private TMP_Text defenseValueText;
    [SerializeField] private TMP_Text ammoValueText;
    [SerializeField] private TMP_Text oreValueText;
    [SerializeField] private Image itemFrame;
    [SerializeField] private Image secondaryItemFrame;
    [SerializeField] private Color ammoNormalColor = Color.white;
    [SerializeField] private Color ammoEmptyColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    private SecondaryWeaponController _secondaryWeaponController;

    private void Start()
    {
        _secondaryWeaponController = FindObjectOfType<SecondaryWeaponController>();

        MeleeController meleeController = FindObjectOfType<MeleeController>();
        if (meleeController != null && meleeController.HasWeapon)
        {
            SetItemFrameImage(meleeController.currentMeleeWeapon.itemFrameImage);
        }
        else
        {
            SetItemFrameImage(null);
        }

        if (_secondaryWeaponController == null || !_secondaryWeaponController.HasWeapon)
        {
            SetSecondaryItemFrameImage(null);
        }

        UpdateStatTexts();

        attackValueText.ForceMeshUpdate();
        defenseValueText.ForceMeshUpdate();
        ammoValueText.ForceMeshUpdate();
        if (oreValueText != null)
        {
            oreValueText.ForceMeshUpdate();
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(attackValueText.transform.parent as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(defenseValueText.transform.parent as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ammoValueText.transform.parent as RectTransform);
        if (oreValueText != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(oreValueText.transform.parent as RectTransform);
        }
    }

    private void Update()
    {
        UpdateStatTexts();
    }

    private void UpdateStatTexts()
    {
        attackValueText.text = PlayerStats.Attack.ToString();
        defenseValueText.text = PlayerStats.Defense.ToString();

        string ammoText = GetSecondaryWeaponAmmoText();
        ammoValueText.text = ammoText;

        bool isEmpty = _secondaryWeaponController == null
            || !_secondaryWeaponController.HasWeapon
            || _secondaryWeaponController.CurrentAmmo <= 0;
        ammoValueText.color = isEmpty ? ammoEmptyColor : ammoNormalColor;

        if (oreValueText != null)
        {
            oreValueText.text = PlayerStats.Ore.ToString("D2");
        }
    }

    private string GetSecondaryWeaponAmmoText()
    {
        if (_secondaryWeaponController == null || !_secondaryWeaponController.HasWeapon)
        {
            return "00";
        }

        return _secondaryWeaponController.CurrentAmmo.ToString("D2");
    }
    public void FillBossHealthMeter()
    {
        bossHealthMeter.FillMeter();
    }

    public void FillPlayerHealthMeter()
    {
        playerHealthMeter.FillMeter();
    }

    public void ReducePlayerHealthMeter(int amount)
    {
        playerHealthMeter.Decrement(amount);
    }

    public void ReduceBossHealthMeter(int amount)
    {
        bossHealthMeter.Decrement(amount);
    }

    public void SetItemFrameImage(Sprite image)
    {
        itemFrame.sprite = image;
        itemFrame.color = image != null ? Color.white : Color.clear;
    }

    public void SetSecondaryItemFrameImage(Sprite image)
    {
        secondaryItemFrame.sprite = image;
        secondaryItemFrame.color = image != null ? Color.white : Color.clear;
    }
}
