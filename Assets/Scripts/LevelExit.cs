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
    public string subzone;
    public OverworldSubzoneContainer.PlayerDirection direction;

    [SerializeField] GameObject faderObject;
    private Image image;

    private void Start()
    {
        if (faderObject != null)
        {
            image = faderObject.GetComponent<Image>();
        }
    }

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

            StartCoroutine(DoSceneExit());

            // Note from Brice:
            // If we want to use the transition animation here, we can use: FindObjectOfType<LevelLoaderTransitions>().LoadNextLevel(nextSceneName);
            // Could probably do on the overworld transtion too
        }
    }

    private IEnumerator DoSceneExit()
    {
        if (image != null)
        {
            while (image.color.a < 1.0f)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.2f);
                yield return new WaitForSeconds(0.08f);
            }

        } else {
            print("No fader object specified on LevelExit script");
        }
        
        SceneManager.LoadScene("Overworld");
    }
}
