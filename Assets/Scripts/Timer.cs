using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timerValue;
    public bool timerRunning = true;
    [SerializeField] public float startingValue = 0f;
    [SerializeField] public float moneyMultiplier = 100f;
    [SerializeField] private TMP_Text timerText;


    // Start is called before the first frame update
    void Start()
    {
        RestartTimer();
        timerText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            timerValue += Time.deltaTime * moneyMultiplier;
        }

        timerText.text = "$" + timerValue.ToString("0");
    }

    public void RestartTimer()
    {
        timerValue = startingValue;
        timerRunning = true;
    }

    public float GetTimerValue()
    {
        return timerValue;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void AddTime(float howMuch)
    {
        timerValue += howMuch;
    }

}
