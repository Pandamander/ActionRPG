using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StorySection
{
    public Sprite image;
    public string animationClipName;
    public string[] storyText;
}

public class StoryboardManagerBeta : MonoBehaviour
{

    public string nextScene = "";
    public GameObject textObject;
    public GameObject imageObject;
    public List<StorySection> storySections = new List<StorySection>(); // a list of storyboard sections - each section can include multiple text strings and an image

    private TMP_Text textField;
    private TextMeshProUGUI textGUI;
    private Image image;
    private Animator imageAnimator;
    private int currentVisibleCharacterIndex;
    private float waitTimeBetweenLetters = 0.05f;
    private float punctuationDelay = 0.5f;
    private float waitTimeAtEnd = 2.0f;

    void Start()
    {
        textField = textObject.GetComponent<TMP_Text>();
        textField.maxVisibleCharacters = 0;
        textGUI = textObject.GetComponent<TextMeshProUGUI>();

        image = imageObject.GetComponent<Image>();
        imageAnimator = imageObject.GetComponent<Animator>();

        StartCoroutine(RunStoryboard());
    }

    IEnumerator RunStoryboard()
    {
        // Loop through the number of sections in the StorySectionList and call the LoadStoryboardSection function
        for (int i = 0; i < storySections.Count; i++)
        {
            yield return StartCoroutine(LoadStoryboardSection(i));
        }

        //yield return new WaitForSeconds(2.0f);

        // after all the sections are finished, load the intro boat scene
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator LoadStoryboardSection(int currentSection)
    {
        // begin the first image transition in
        yield return StartCoroutine(SteppedImageFadeIn());

        // loop through the text in a section and do the typewriter effect on it
        for (int i = 0; i < storySections[currentSection].storyText.Length; i++)
        {

            if (storySections[currentSection].image != null)
            {
                image.sprite = storySections[currentSection].image;
            }

            if (storySections[currentSection].animationClipName != null)
            {
                imageAnimator.Play(storySections[currentSection].animationClipName);
            }

            textField.text = storySections[currentSection].storyText[i];
            yield return StartCoroutine(TypeWriterEffect());
        }

        StartCoroutine(SteppedImageFadeOut());
        yield return StartCoroutine(SteppedTextFadeOut());
    }

    private IEnumerator SteppedImageFadeIn ()
    {
        while (image.color.a < 1.0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.25f);
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    private IEnumerator SteppedImageFadeOut()
    {
        while (image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.25f);
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    private IEnumerator SteppedTextFadeOut()
    {
        while (textGUI.alpha > 0)
        {
            textGUI.alpha = textGUI.alpha - 0.25f;
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    private IEnumerator TypeWriterEffect()
    {

        textField.maxVisibleCharacters = 0; // this makes it so there are no characters visible in the beginning
        currentVisibleCharacterIndex = 0;

        textField.ForceMeshUpdate(); // this forces the text to get rendered into the text mesh so this script can get an accurate character count
        TMP_TextInfo textInfo = textField.textInfo;

        // loop through all text characters and gradually reveal each one
        while (currentVisibleCharacterIndex < textInfo.characterCount + 1)
        {
            int lastCharacterIndex = textInfo.characterCount - 1;

            // are we at the end yet?
            if (currentVisibleCharacterIndex >= lastCharacterIndex)
            {
                textField.maxVisibleCharacters++;
                yield return new WaitForSeconds(waitTimeAtEnd);
                yield break;
            }

            char character = textInfo.characterInfo[currentVisibleCharacterIndex].character;

            textField.maxVisibleCharacters++;

            // add extra delays for punctuation
            if (character == '?' || character == '.' || character == ',' || character == ':' ||
                 character == ';' || character == '!' || character == '-')
            {
                yield return new WaitForSeconds(punctuationDelay);
            }

            currentVisibleCharacterIndex++;
            yield return new WaitForSeconds(waitTimeBetweenLetters);
        }

        yield return new WaitForSeconds(waitTimeAtEnd);
    }



}
