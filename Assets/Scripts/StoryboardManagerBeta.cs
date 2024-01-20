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
    private float waitTimeAtEnd = 2.0f;

    // stuff related to the skip cutscene UI/UX
    [SerializeField] private Image skipCutsceneRadialIndicator;
    [SerializeField] private KeyCode skipKey = KeyCode.E;
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

        StartCoroutine(RunStoryboard());
    }

    private void Update()
    {

        // This is a bunch of stuff for skipping the cutscene and showing a
        // radial progress indicator by pressing and holding a key

        if (!skippingCutscene && Input.GetKeyDown(skipKey))
        {
            skipKeyPressed = true;
        }

        if (!skippingCutscene && Input.GetKeyUp(skipKey))
        {
            skipKeyPressed = false;
        }

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
                radialSkipIndicatorTimer += Time.deltaTime;
                skipCutsceneRadialIndicator.fillAmount = radialSkipIndicatorTimer;
            }

        }
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
