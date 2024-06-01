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
    private Fader _fader;

    private void Awake()
    {
        _fader = GetComponentInChildren<Fader>();
    }
    private void Start()
    {
        _fader.FadeOut();
    }

    private void Update()
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
}
