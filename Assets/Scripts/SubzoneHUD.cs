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
    [SerializeField] private Image itemFrame;
    [SerializeField] private Image secondaryItemFrame;

    private void Start()
    {
        UpdateStatTexts();

        attackValueText.ForceMeshUpdate();
        defenseValueText.ForceMeshUpdate();
        LayoutRebuilder.ForceRebuildLayoutImmediate(attackValueText.transform.parent as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(defenseValueText.transform.parent as RectTransform);
    }

    private void Update()
    {
        UpdateStatTexts();
    }

    private void UpdateStatTexts()
    {
        attackValueText.text = PlayerStats.Attack.ToString();
        defenseValueText.text = PlayerStats.Defense.ToString();
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
    }

    public void SetSecondaryItemFrameImage(Sprite image)
    {
        secondaryItemFrame.sprite = image;
    }
}
