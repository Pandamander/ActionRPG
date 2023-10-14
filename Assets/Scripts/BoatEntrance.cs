using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEntrance : MonoBehaviour
{
    [SerializeField] private Transform boat;
    [SerializeField] private Transform hero;
    [SerializeField] private BoxCollider2D heroCollider;
    [SerializeField] private TopDownMovement heroTopDownMovement;

    private bool _shouldMoveBoat = false;
    private float _boatMoveSpeed = 1f;

    private IEnumerator MoveTo(Transform t, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = t.position;
        while (elapsedTime < seconds)
        {
            t.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t.position = end;
    }

    private void Update()
    {
        if (_shouldMoveBoat)
        {
            boat.position = new Vector3(boat.position.x, boat.position.y + Time.deltaTime * _boatMoveSpeed, 0f);
        }
    }

    private IEnumerator FinalSequence()
    {
        heroTopDownMovement.AnimateWalkingRight();
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(MoveTo(hero, boat.position, 2f));
        hero.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        _shouldMoveBoat = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OverworldHero"))
        {
            heroCollider.enabled = false;
            StartCoroutine(FinalSequence());
        }
    }
}
