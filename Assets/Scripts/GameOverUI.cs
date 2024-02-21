using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private Image menuCursor;
    [SerializeField] private List<GameObject> menuCursorPositions = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> menuItemsText = new List<TextMeshProUGUI>();
    [SerializeField] private Color inactiveMenuTextColor;
    [SerializeField] private Color activeMenuTextColor;

    private List<Action> menuActions = new List<Action>(); // a list of delegate functions for menu actions

    private int currentMenuItemIndex = 0;

    private float axisInputDelayDuration = 0.5f; // add a delay of .5 seconds between switching menu items when holding a direction
    private float elapsedTimeSinceAxisInput = 0;
    private bool acceptingAxisInputUp = true;
    private bool acceptingAxisInputDown = true;

    void Start()
    {

        // set starting position of the menu cursor to first menu item
        menuCursor.transform.position = menuCursorPositions[0].transform.position;

        // Add functions to the menu actions list
        menuActions.Add(() => GameManager.sharedInstance.ContinueFromGameOver());
        menuActions.Add(() => GameManager.sharedInstance.QuitGame());
    }

    void Update()
    {

        float inputVertical = Input.GetAxisRaw("Vertical");

        // axis input down
        if (inputVertical < 0 && acceptingAxisInputDown == true)
        {
            acceptingAxisInputDown = false;
            acceptingAxisInputUp = true; // should immediately be able to move up after pressing down

            StartCoroutine(DelayAxisInputDown());

            currentMenuItemIndex--;
            // if it goes below the first menu item, then start at the top
            if (currentMenuItemIndex < 0)
            {
                currentMenuItemIndex = menuActions.Count - 1;
            }

            // move cursor to the next menu item
            menuCursor.transform.position = new Vector3(menuCursorPositions[currentMenuItemIndex].transform.position.x, menuCursorPositions[currentMenuItemIndex].transform.position.y, menuCursorPositions[currentMenuItemIndex].transform.position.z);
            menuCursor.gameObject.GetComponent<Animator>().Play("Menu Cursor", -1, 0f);

            // update all menu item text colors
            UpdateMenuItemsTextColors();

        }

        // axis input up
        else if (inputVertical > 0 && acceptingAxisInputUp == true)
        {
            acceptingAxisInputUp = false;
            acceptingAxisInputDown = true;

            StartCoroutine(DelayAxisInputUp());

            currentMenuItemIndex++;
            // if it goes past the last menu item, then start back at the beginning
            if (currentMenuItemIndex > (menuActions.Count - 1))
            {
                currentMenuItemIndex = 0;
            }

            // move cursor to the next menu item
            menuCursor.transform.position = new Vector3(menuCursorPositions[currentMenuItemIndex].transform.position.x, menuCursorPositions[currentMenuItemIndex].transform.position.y, menuCursorPositions[currentMenuItemIndex].transform.position.z);
            menuCursor.gameObject.GetComponent<Animator>().Play("Menu Cursor", -1, 0f);

            // update all menu item text colors
            UpdateMenuItemsTextColors();
        }

        // confirm button
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.X))
        {
            //do menu action based on currently selected menu item
            menuActions[currentMenuItemIndex]();
        }

        // reset rate limit when releasing a direction
        if (inputVertical == 0)
        {
            acceptingAxisInputDown = true;
            acceptingAxisInputUp = true;
        }
    }

    private void UpdateMenuItemsTextColors()
    {
        for (int i = 0; i < menuItemsText.Count; i++)
        {
            if (i == currentMenuItemIndex)
            {
                menuItemsText[i].color = activeMenuTextColor;
            }
            else
            {
                menuItemsText[i].color = inactiveMenuTextColor;
            }
        }
    }

    private IEnumerator DelayAxisInputDown()
    {

        elapsedTimeSinceAxisInput = 0;

        while (elapsedTimeSinceAxisInput < axisInputDelayDuration)
        {
            elapsedTimeSinceAxisInput += Time.deltaTime;
            yield return null;
        }

        acceptingAxisInputDown = true;

        yield return null;
    }

    private IEnumerator DelayAxisInputUp()
    {

        elapsedTimeSinceAxisInput = 0;

        while (elapsedTimeSinceAxisInput < axisInputDelayDuration)
        {
            elapsedTimeSinceAxisInput += Time.deltaTime;
            yield return null;
        }

        acceptingAxisInputUp = true;

        yield return null;
    }

    // based on Robert Penner's easing functions,
    // takes the current lerp time value and interpolates it to a quartic ease out curve
    private float QuarticEaseOut(float t)
    {
        float f = (t - 1);
        return f * f * f * (1 - t) + 1;
    }
}
