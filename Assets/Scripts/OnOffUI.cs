using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class OnOffUI : MonoBehaviour
{
    [SerializeField] GameObject GameHUDUI;
    [SerializeField] GameObject EndUIToTurnOffOn;
    public TMP_Text MoneyValueText;
    public TMP_Text CriminalityValueText;
    public TMP_Text PlayAgainText;
    private bool BlinkText = false;
    private float textTimer = 0.0f;

    // This script handles tracking the score and also the end of the game

    void Start()
    {
        EndUIToTurnOffOn.SetActive(false);
        GameHUDUI.SetActive(true);
    }

    private void Update()
    {
        if (BlinkText)
        {
            textTimer += Time.deltaTime;
            if (textTimer >= 0.5f)
            {
                PlayAgainText.gameObject.SetActive(!PlayAgainText.gameObject.activeInHierarchy);
                textTimer = 0.0f;
            }
        }
    }

    public void HideUI()
    {
        EndUIToTurnOffOn.SetActive(false);
    }

    public void ShowUI()
    {
        EndUIToTurnOffOn.SetActive(true);
        GameHUDUI.SetActive(false);
    }

    public void ShowEndGameUI(float moneyScore)
    {
        ShowUI();
        MoneyValueText.text = "$" + moneyScore.ToString("0");
        CriminalityValueText.text = CriminalityTier(moneyScore);
        BlinkText = true;
    }

    private string CriminalityTier(float moneyScore)
    {
        string[] tiers = { "F", "D", "C", "B", "A", "S" };

        float moneyTier = 10000.0f;

        int tierIndex = (int)(moneyScore / moneyTier);

        tierIndex = Math.Clamp(tierIndex, 0, tiers.Length - 1);

        return tiers[tierIndex];
    }
}
