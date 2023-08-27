using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    // Override position for Overworld placement when exiting this level
    public Vector3 levelExitOverworldPositionOverride;
    public bool usePositionOverride;
    public string subzone;
    public OverworldSubzoneContainer.PlayerDirection direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (usePositionOverride)
            {
                OverworldSubzoneContainer.AddEncounter(
                    levelExitOverworldPositionOverride.x,
                    levelExitOverworldPositionOverride.y,
                    subzone,
                    direction
                );
            }
            SceneManager.LoadScene("Overworld");
            // Note from Brice:
            // If we want to use the transition animation here, we can use: FindObjectOfType<LevelLoaderTransitions>().LoadNextLevel(nextSceneName);
            // Could probably do on the overworld transtion too
        }
    }
}


// Vector3(-5.82999992,-2.82999992,0)
