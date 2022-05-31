using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter : MonoBehaviour
{
    public string subzoneName;
    public string enemyName;
    public string uniqueTag;
    public OverworldAudioManager audioManager;
    public GridMovement heroMovement;
    private EnemyGridMovement enemyMovement;
    private Transform movePoint;

    private void Awake()
    {
        movePoint = transform.Find("MovePoint");
        enemyMovement = gameObject.GetComponent<EnemyGridMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OverworldHero"))
        {
            audioManager.PlayEnemyEncounter();
            OverworldSubzoneContainer.AddEncounter(
                movePoint.position.x,
                movePoint.position.y,
                subzoneName,
                enemyName,
                uniqueTag
            );

            heroMovement.StopMovement();
            enemyMovement.StopMovement();
            StartCoroutine(SubzoneTransition());
        }
    }

    private IEnumerator SubzoneTransition()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(subzoneName);
    }
}
