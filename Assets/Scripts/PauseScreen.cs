using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreenPrefab;
    private bool _isPaused = false;
    private GameObject _pauseScreen;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _pauseScreen = GameObject.Instantiate(pauseScreenPrefab);
        DontDestroyOnLoad(_pauseScreen);
        _pauseScreen.SetActive(false);
        _isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            _pauseScreen.SetActive(!_isPaused);
            _isPaused = _pauseScreen.activeInHierarchy;
        }
        if (Input.GetButtonDown("Fire2") && _isPaused)
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}