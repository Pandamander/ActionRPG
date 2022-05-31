using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubzoneEntrance : MonoBehaviour
{
    public string subzone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OverworldHero"))
        {
            OverworldSubzoneContainer.AddEncounter(
                gameObject.transform.position.x,
                gameObject.transform.position.y,
                subzone,
                "",
                ""
            );

            SceneManager.LoadScene(subzone);
        }
    }

}
