using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HUDHealthMeter : MonoBehaviour
{
    [SerializeField] private GameObject[] healthMeterFilled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fill()
    {
        foreach (GameObject bar in healthMeterFilled)
        {
            if (bar.activeInHierarchy == false)
            {
                StartCoroutine(FillOne(bar));
            }
        }
    }

    private IEnumerator FillOne(GameObject bar)
    {
        yield return new WaitForSeconds(0.25f);
        bar.SetActive(true);
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
