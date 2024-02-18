using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdvanceScene : MonoBehaviour
{
    [SerializeField] bool anyKeyToAdvance = true;
    [SerializeField] bool spaceToAdvance;
    [SerializeField] bool returnToAdvance;

    [SerializeField] public string nextSceneName;
    [SerializeField] public float delay = 5f;
    //[SerializeField] bool autoAdvance = false;

    [SerializeField] GameObject faderObject;
    private Image image;
    private bool isAdvancing = false;

    // Start is called before the first frame update
    void Start()
    {
        //if (autoAdvance)
            //Invoke("NextScene", delay);

        if (faderObject != null)
        {
            image = faderObject.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
        
    }

    private IEnumerator NextScene()
    {
        isAdvancing = true;
        //FindObjectOfType<AudioManager>().Play("MenuClick");

        // do a fade out if the image exists
        if (image != null)
        {
            while (image.color.a < 1.0f)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.2f);
                yield return new WaitForSeconds(0.08f);
            }
            yield return new WaitForSeconds(2.0f); // short delay after fade out before advancing
        } else {
            print("No fader object specified on AdvanceScene script");
        }

        SceneManager.LoadScene(nextSceneName);

        yield return null;
    }

    private void Update()
    {
        // Don't trigger game start from showing Pause Menu
        if (Input.GetButtonDown("Cancel")) return;

        if (Input.anyKey && anyKeyToAdvance && isAdvancing == false)
        {
            StartCoroutine(NextScene());
        }
        if (Input.GetKeyDown(KeyCode.Space) && spaceToAdvance && isAdvancing == false)
        {
            StartCoroutine(NextScene());
        }
        if (Input.GetKeyDown(KeyCode.Return) && returnToAdvance && isAdvancing == false)
        {
            StartCoroutine(NextScene());
        }

    }
}
