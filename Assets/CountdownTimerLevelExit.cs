using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownTimerLevelExit : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 10f;
    [SerializeField] private string nextSceneName = "Subzone Intro 1";
    bool timerHasBeenCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Count down timeRemaining by deltaTime. When it reaches 0, then load the next scene.
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0 && !timerHasBeenCalled)
        {
            timerHasBeenCalled = true;
            // Find the LevelLoaderTransitions and load the next scene
            FindObjectOfType<LevelLoaderTransitions>().LoadNextLevel(nextSceneName);

        }

    }
}
