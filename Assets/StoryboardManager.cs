using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryboardManager : MonoBehaviour
{
    // Array of Images
    public Sprite[] images;
    public string[] storyText;
    public Image mainStoryboardImage;
    public TMP_Text mainStoryboardText;
    public Animator Imageanimator;
    public Animator textAnimator;

    // Start is called before the first frame update
    void Start()
    {
        mainStoryboardText.text = "HOOOOO";
        StartCoroutine(RunStoryboard());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator RunStoryboard()
    {
        // Loop through the number of images in the array and call the LoadStoryboardSection function
        for (int i = 0; i < images.Length; i++)
        {
            yield return StartCoroutine(LoadStoryboardSection(i));
        }
        textAnimator.SetTrigger("FadeOutText");
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("Subzone Intro Boat");
    }

    IEnumerator LoadStoryboardSection(int sectionNumber)
    {
        // Load mainStoryboardImage with the image from the array
        mainStoryboardImage.sprite = images[sectionNumber];
        Imageanimator.SetTrigger("FadeInTrigger");

        yield return StartCoroutine(TypeWriterEffect(storyText[sectionNumber], 0.05f, 2f));
    }

    IEnumerator TypeWriterEffect(string text, float waitTimeBetweenLetters, float waitTimeAtEnd)
    {
        mainStoryboardText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            mainStoryboardText.text += letter;
            yield return new WaitForSeconds(waitTimeBetweenLetters);
        }

        yield return new WaitForSeconds(waitTimeAtEnd);
        Imageanimator.SetTrigger("FadeOutTrigger");
        yield return new WaitForSeconds(waitTimeAtEnd);
    }




}
