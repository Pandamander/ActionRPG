using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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
    private float waitTimeAtEnd = 3.5f; // the time between the end of a section of text and the beginning of another

    // stuff related to the skip cutscene UI/UX
    [SerializeField] private Image skipCutsceneRadialIndicator;
    [SerializeField] private Image skipKeyGraphicResting;
    [SerializeField] private Image skipKeyGraphicPressed;
    [SerializeField] private KeyCode skipKey = KeyCode.E;
    [SerializeField] private TextMeshProUGUI skipTextLabel;
    [SerializeField] private Color skipTextLabelActiveColor = new Color(1.0f, 1.0f, 1.0f);
    private Color skipTextLabelInactiveColor;
    [SerializeField] private Image skipCutsceneFadeBG;

    private float radialSkipIndicatorTimer = 0;
    private float radialSkipIndicatorTimerMax = 1.0f;
    private bool skipKeyPressed = false;
    private bool skippingCutscene = false;

    void Start()
    {
        textField = textObject.GetComponent<TMP_Text>();
        textField.maxVisibleCharacters = 0;
        textGUI = textObject.GetComponent<TextMeshProUGUI>();

        image = imageObject.GetComponent<Image>();
        imageAnimator = imageObject.GetComponent<Animator>();

        skipTextLabelInactiveColor = skipTextLabel.color;

        StartCoroutine(RunStoryboard());
    }

    private void Update()
    {

        // This is a bunch of stuff for skipping the cutscene and showing a
        // radial progress indicator by pressing and holding a key

        // skip key pressed
        if (!skippingCutscene && Input.GetKeyDown(skipKey))
        {
            skipTextLabel.color = skipTextLabelActiveColor;
            skipKeyPressed = true;
            skipKeyGraphicResting.gameObject.SetActive(false);
            skipKeyGraphicPressed.gameObject.SetActive(true);
        }

        // skip key released
        if (!skippingCutscene && Input.GetKeyUp(skipKey))
        {
            skipTextLabel.color = skipTextLabelInactiveColor;
            skipKeyPressed = false;
            skipKeyGraphicResting.gameObject.SetActive(true);
            skipKeyGraphicPressed.gameObject.SetActive(false);
        }

        // skipping in progress
        if (!skippingCutscene && skipKeyPressed)
        {

            if (radialSkipIndicatorTimer >= radialSkipIndicatorTimerMax)
            {
                // skip cutscene here
                radialSkipIndicatorTimer = radialSkipIndicatorTimerMax;
                skippingCutscene = true;
                StartCoroutine(SkipCutscene());
            }
            else
            {
                // fill radial skip progress indicator
                radialSkipIndicatorTimer += Time.deltaTime;
                skipCutsceneRadialIndicator.fillAmount = radialSkipIndicatorTimer;
            }

        }

        // this reduces any existing radial fill on the skip progress bar when the skip key is released
        else if (!skippingCutscene && !skipKeyPressed && skipCutsceneRadialIndicator.fillAmount > 0)
        {
            radialSkipIndicatorTimer -= Time.deltaTime;
            skipCutsceneRadialIndicator.fillAmount = radialSkipIndicatorTimer;
        }
    }

    IEnumerator RunStoryboard()
    {
        // Loop through the number of sections in the StorySectionList and call the LoadStoryboardSection function
        for (int i = 0; i < storySections.Count; i++)
        {
            yield return StartCoroutine(LoadStoryboardSection(i));
        }

        yield return new WaitForSeconds(2.0f); // wait a couple seconds before going to the next scene

        // after all the sections are finished, load the intro boat scene
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator LoadStoryboardSection(int currentSection)
    {
        // if it's the first image, do a stepped fade in
        if (currentSection == 0)
        {
            yield return StartCoroutine(SteppedImageFadeIn(0.5f));
        }
        // if not the first image, then do a regular fade in
        else {
            StartCoroutine(ImageFadeIn(0.5f));
        }

        textGUI.alpha = 1.0f;

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

        // if the last section, do a stepped fade out
        if (currentSection == storySections.Count - 1)
        {
            StartCoroutine(SteppedImageFadeOut(0.5f));
            yield return StartCoroutine(SteppedTextFadeOut(0.5f));
        }
        // otherwise just immediately set the image alpha to 0
        else {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
        
    }

    private IEnumerator ImageFadeOut(float duration)
    {
        float elapsedTime = 0;

        while (image.color.a > 0)
        {
            float newAlpha = Mathf.Lerp(1.0f, 0, elapsedTime / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator ImageFadeIn(float duration)
    {
        float elapsedTime = 0;

        while (image.color.a < 1.0f)
        {
            float newAlpha = Mathf.Lerp(0, 1.0f, elapsedTime / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator TextFadeOut(float duration)
    {
        float elapsedTime = 0;

        while (textGUI.alpha > 0)
        {
            float newAlpha = Mathf.Lerp(1.0f, 0, elapsedTime / duration);
            textGUI.alpha = newAlpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator SteppedImageFadeIn (float timeBetweenSteps)
    {
        while (image.color.a < 1.0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.25f);
            yield return new WaitForSeconds(timeBetweenSteps);
        }

        yield return null;
    }

    private IEnumerator SteppedImageFadeOut(float timeBetweenSteps)
    {
        while (image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.25f);
            yield return new WaitForSeconds(timeBetweenSteps);
        }

        yield return null;
    }

    private IEnumerator SteppedTextFadeOut(float timeBetweenSteps)
    {
        while (textGUI.alpha > 0)
        {
            textGUI.alpha = textGUI.alpha - 0.25f;
            yield return new WaitForSeconds(timeBetweenSteps);
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

    private IEnumerator SkipCutscene()
    {
        yield return StartCoroutine(DoSkipCutsceneFade());
        SceneManager.LoadScene(nextScene);
        yield return null;
    }

    private IEnumerator DoSkipCutsceneFade ()
    {
        float fadeDuration = 1.0f;
        float elapsedTime = 0;
        float alpha = 0;

        while (alpha < 1.0f)
        {
            //print ("alpha is " + alpha);
            //print("elapsed time is " + elapsedTime);
            alpha = Mathf.Lerp(0, 1.0f, elapsedTime / fadeDuration);
            skipCutsceneFadeBG.color = new Color(skipCutsceneFadeBG.color.r, skipCutsceneFadeBG.color.g, skipCutsceneFadeBG.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

}
