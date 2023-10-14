using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverworldInitialization : MonoBehaviour
{
    [SerializeField] private Fader fader;

    private void Awake()
    {
        PlayerStats.Initialize();
        OverworldSubzoneContainer.Initialize();

        OverworldDestroy();
    }

    private void Start()
    {
        fader.FadeOut();
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
