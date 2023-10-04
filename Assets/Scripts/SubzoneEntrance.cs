using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubzoneEntrance : MonoBehaviour
{
    public string subzone;
    public Vector3 positionOverride;
    public bool usePositionOverride;

    public Vector3 subzoneLevelStartPositionOverride;
    public bool usesubzoneLevelStartPositionOverride;

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
        if (collision.gameObject.CompareTag("OverworldHero"))
        {
            // stop hero movement
            collision.GetComponent<TopDownMovement>().StopMovement();

            Vector3 encounterPosition = usePositionOverride ?
                positionOverride : gameObject.transform.position;
            OverworldSubzoneContainer.AddEncounter(
                encounterPosition.x,
                encounterPosition.y - 1, // Offset player from last encounter so we don't auto-collide again.
                subzone,
                OverworldSubzoneContainer.PlayerDirection.Down
            );

            if (usesubzoneLevelStartPositionOverride)
            {
                OverworldSubzoneContainer.AddSubzoneStartPosition(
                    subzoneLevelStartPositionOverride.x,
                    subzoneLevelStartPositionOverride.y,
                    OverworldSubzoneContainer.PlayerDirection.Left
                );
            }

            StartCoroutine(DoSceneExit());
        }
    }

    // contains effect for fading to black and call to load the next scene
    private IEnumerator DoSceneExit()
    {
        if (image != null)
        {
            while (image.color.a < 1.0)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.2f);
                yield return new WaitForSeconds(0.08f);
            }
        }

        SceneManager.LoadScene(subzone);

        yield return null;
    }
}
