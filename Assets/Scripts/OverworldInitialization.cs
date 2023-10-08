using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverworldInitialization : MonoBehaviour
{
    private void Awake()
    {
        PlayerStats.Initialize();
        OverworldSubzoneContainer.Initialize();

        OverworldDestroy();
    }

    private void OverworldDestroy()
    {
        foreach (string tag in PlayerStats.OverworldDestroyList)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
            {
                Destroy(obj);
            }
        }
    }
}
