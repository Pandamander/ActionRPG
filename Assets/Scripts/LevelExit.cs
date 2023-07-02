using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("LevelExit OnTriggerEnter2D: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Overworld");
            // Note from Brice:
            // If we want to use the transition animation here, we can use: FindObjectOfType<LevelLoaderTransitions>().LoadNextLevel(nextSceneName);
            // Could probably do on the overworld transtion too
        }
    }
}


// Vector3(-5.82999992,-2.82999992,0)
