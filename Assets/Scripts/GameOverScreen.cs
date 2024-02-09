using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreenPrefab;
    private bool _isPaused = false;
    private GameObject _gameOverScreen;
    private string continueLevelName;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _gameOverScreen = GameObject.Instantiate(pauseScreenPrefab);
        DontDestroyOnLoad(_gameOverScreen);
        _gameOverScreen.SetActive(false);
        _isPaused = false;
    }

    public void ShowGameOver(string levelName)
    {
        continueLevelName = levelName;
        _gameOverScreen.SetActive(true);
        _isPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && _isPaused)
        {
            _gameOverScreen.SetActive(!_isPaused);
            _isPaused = false;
            PlayerStats.ResetHealthForContinue();
            SceneManager.LoadScene(continueLevelName);
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
