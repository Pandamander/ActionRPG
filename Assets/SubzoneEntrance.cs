using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubzoneEntrance : MonoBehaviour
{
    public string subzone;
    public Vector2 position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OverworldHero"))
        {
            OverworldSubzoneContainer.AddEncounter(
                position.x,
                position.y,
                subzone,
                "",
                ""
            );

            SceneManager.LoadScene(subzone);
        }
    }

}
