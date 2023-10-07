using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    List<GameObject> trailParts = new List<GameObject>();

    public void StartTrail()
    {
        InvokeRepeating("SpawnTrailPart", 0, 0.1f);
    }

    void SpawnTrailPart()
    {
        GameObject trailPart = new GameObject();
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        trailPartRenderer.sortingOrder = 10;
        trailPartRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        trailPart.transform.position = transform.position;
        trailPart.transform.localScale = transform.localScale;
        trailParts.Add(trailPart);

        StartCoroutine(FadeTrailPart(trailPartRenderer));
        Destroy(trailPart, 0.5f); // lifetime of ghost
    }

    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        Color color = trailPartRenderer.color;// Random.ColorHSV(.5f, 1f, 1f, 1f, 0.8f, 1f, 1f, 1f);
        color.a -= 0.5f;
        trailPartRenderer.color = color;

        yield return new WaitForEndOfFrame();
    }
}
