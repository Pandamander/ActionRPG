using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubzoneEntrance : MonoBehaviour
{
    public string subzone;
    public Vector3 positionOverride;
    public bool usePositionOverride;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OverworldHero"))
        {
            Vector3 encounterPosition = usePositionOverride ?
                positionOverride : gameObject.transform.position;
            OverworldSubzoneContainer.AddEncounter(
                encounterPosition.x,
                encounterPosition.y,
                subzone
            );

            SceneManager.LoadScene(subzone);
        }
    }

}
