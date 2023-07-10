using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderTransitions : MonoBehaviour
{

    public Animator transitionAnimator;
    public float transitionTime = 3f;
    // Brice made this script to do scene transitions. We should probably merge this in with the other Level Loader scripts we already have
    public AudioSource source;
    

    public void LoadNextLevel(string nextSceneName)
    {
        // Load the next scene
        StartCoroutine(LoadLevel(nextSceneName));
    }

    IEnumerator LoadLevel(string LevelName)
    {
        // Play animation
        transitionAnimator.SetTrigger("Start");

        source.Play();

        // Wait
        yield return new WaitForSeconds(transitionTime); // Pauses this coroutine for 1 second before continuing

        // Load level
        SceneManager.LoadScene(LevelName);
    }
}
