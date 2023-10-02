using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWave : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject sandPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnWave()
    {
        Instantiate(sandPrefab, spawnPoints[0].position, Quaternion.identity);
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for (int i = 1; i < spawnPoints.Length; i++)
        {
            yield return StartCoroutine(SpawnSand(spawnPoints[i]));
        }
    }

    private IEnumerator SpawnSand(Transform t)
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(sandPrefab, t.position, Quaternion.identity);
    }
}
