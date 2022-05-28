using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter : MonoBehaviour
{
    public string subzoneName;
    public string enemyName;
    private Transform movePoint;

    private void Awake()
    {
        movePoint = transform.Find("MovePoint");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OverworldHero"))
        {
            OverworldSubzoneContainer.AddEncounter(
                movePoint.position.x,
                movePoint.position.y,
                subzoneName,
                enemyName
            );

            SceneManager.LoadScene(subzoneName);
        }
    }
}
