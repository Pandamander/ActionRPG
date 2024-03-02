using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    // Override position for Overworld placement when exiting this level
    public Vector3 levelExitOverworldPositionOverride;
    public bool usePositionOverride;

    // Overrides if loading a Subzone from a Subzone
    public Vector3 subzoneLevelStartPositionOverride;
    public bool useSubzoneLevelStartPositionOverride;
    public OverworldSubzoneContainer.PlayerDirection subzoneLevelStartDirectionOverride = OverworldSubzoneContainer.PlayerDirection.Left;

    public string subzone;
    public string levelToLoadOnExit = "Overworld";
    public OverworldSubzoneContainer.PlayerDirection direction;

    [SerializeField] private Fader fader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // stop player movement and animation
            collision.gameObject.GetComponent<PlayerMovement>().FreezeWalking();
            collision.gameObject.GetComponent<Animator>().speed = 0;

            if (usePositionOverride)
            {
                OverworldSubzoneContainer.AddEncounter(
                    levelExitOverworldPositionOverride.x,
                    levelExitOverworldPositionOverride.y,
                    subzone,
                    direction
                );
            }

            if (useSubzoneLevelStartPositionOverride)
            {
                OverworldSubzoneContainer.AddSubzoneStartPosition(
                    subzoneLevelStartPositionOverride.x,
                    subzoneLevelStartPositionOverride.y,
                    subzoneLevelStartDirectionOverride
                );
            }

            StartCoroutine(DoSceneExit());
        }
    }

    private IEnumerator DoSceneExit()
    {
        yield return StartCoroutine(fader.DoFadeIn());
        
        SceneManager.LoadScene(levelToLoadOnExit);

        yield return null;
    }
}
