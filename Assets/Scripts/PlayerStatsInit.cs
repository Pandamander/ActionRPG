using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsInit : MonoBehaviour
{
    private void Awake()
    {
        PlayerStats.Initialize();
        OverworldSubzoneContainer.Initialize();
    }
}
