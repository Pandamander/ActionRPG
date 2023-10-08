using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubzoneHUD : MonoBehaviour
{
    [SerializeField] private HUDHealthMeter playerHealthMeter;
    [SerializeField] private HUDHealthMeter bossHealthMeter;

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
}
