using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubzoneHUD : MonoBehaviour
{
    public TMP_Text attackValue;
    public TMP_Text defenseValue;
    [SerializeField] private HUDHealthMeter playerHealthMeter;
    [SerializeField] private HUDHealthMeter bossHealthMeter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackValue.text = PlayerStats.Attack.ToString();
        defenseValue.text = PlayerStats.Defense.ToString() + "/" + PlayerStats.DefenseCapacity.ToString();
    }

    public void FillBossHealthMeter()
    {
        bossHealthMeter.Fill();
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
