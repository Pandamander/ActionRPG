using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubzoneEntrance : MonoBehaviour
{
    public string subzone;
    public Vector3 positionOverride;
    public bool usePositionOverride;

    public Vector3 subzoneLevelStartPositionOverride;
    public bool usesubzoneLevelStartPositionOverride;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OverworldHero"))
        {
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

            SceneManager.LoadScene(subzone);
        }
    }

}
