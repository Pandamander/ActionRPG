using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubzoneHUD : MonoBehaviour
{
    public TMP_Text attackValue;
    public TMP_Text defenseValue;
    public TMP_Text healthValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackValue.text = PlayerStats.Attack.ToString();
        defenseValue.text = PlayerStats.Defense.ToString();
        healthValue.text = PlayerStats.Health.ToString() + "/" + PlayerStats.HealthCapacity.ToString();
    }
}
