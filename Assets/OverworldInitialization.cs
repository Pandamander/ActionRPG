using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldInitialization : MonoBehaviour
{
    private void Awake()
    {
        PlayerStats.Initialize();
        OverworldSubzoneContainer.Initialize();
    }
}
