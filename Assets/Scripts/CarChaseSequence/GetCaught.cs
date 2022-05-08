using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetCaught : MonoBehaviour
{
    [SerializeField] private OnOffUI endingUI;
    [SerializeField] private Timer timer;
    private PlayerInputController input;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInputController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R) && input.canStillMove == false) // Reset if you've been caught
        {
            RestartGame();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "TheLaw") // Get caught
        {
            AudioManager.Instance.PlayCrash();
            AudioManager.Instance.StopPlayingAccelerate();
            EndOfGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TheLaw") // Get caught
        {
            EndOfGame();
        }
    }

    private void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void EndOfGame()
    {
        timer.StopTimer();
        endingUI.ShowEndGameUI(timer.timerValue);
        input.Stop();
    }
}
