using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HUDHealthMeter : MonoBehaviour
{
    public bool isPlayer = false;
    [SerializeField] private GameObject[] healthMeterFilled;
    [SerializeField] private SubzoneAudioManager subzoneAudioManager;

    private void Awake()
    {
        // Set to empty health and let level managers / PlayerStats update
        for (int i = healthMeterFilled.Length; i-- > 0;)
        {
            healthMeterFilled[i].SetActive(false);
        }
    }

    private void Start()
    {
        // Set player health to current stat health when play starts
        if (isPlayer && (PlayerStats.Health <= healthMeterFilled.Length) )
        {
            SetHealth(PlayerStats.Health);
        }
    }

    public void FillMeter()
    {
        StartCoroutine(Fill());
    }

    public void SetHealth(int health)
    {
        for (int i = 0; i < health; i++)
        {
            healthMeterFilled[i].SetActive(true);
        }
    }

    private IEnumerator Fill()
    {
        foreach (GameObject bar in healthMeterFilled)
        {
            if (bar.activeInHierarchy == false)
            {
                yield return StartCoroutine(FillOne(bar));
            }
        }
    }

    private IEnumerator FillOne(GameObject bar)
    {
        yield return new WaitForSeconds(0.15f);
        if (!bar.activeInHierarchy)
        {
            subzoneAudioManager.PlayArcadeJump();
            bar.SetActive(true);
        }
    }

    public void Increment(int number)
    {
        foreach (GameObject bar in healthMeterFilled)
        {
            if (bar.activeInHierarchy == false)
            {
                bar.SetActive(true);
                number--;
                if (number == 0) { return; }
            }
        }
    }

    public void Decrement(int number)
    {
        for (int i = healthMeterFilled.Length; i-- > 0;)
        {
            if (healthMeterFilled[i].activeInHierarchy == true)
            {
                healthMeterFilled[i].SetActive(false);
                number--;
                if (number == 0) { return; }
            }
        }
    }
}
