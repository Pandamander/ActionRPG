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
    

    void Start()
    {

        // set starting position of the menu cursor to first menu item
        menuCursor.transform.position = menuCursorPositions[0].transform.position;

        // Add functions to the menu actions list
        menuActions.Add(() => GameManager.sharedInstance.ContinueFromGameOver());
        menuActions.Add(() => GameManager.sharedInstance.QuitGame());
    }

    // Update is called once per frame
    void Update()
    {

        float inputVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
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

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMenuItemIndex++;
            // if it goes above the last menu item, then start back at the beginning
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

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.X))
        {
            //do menu action based on currently selected menu item
            menuActions[currentMenuItemIndex]();
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

    // based on Robert Penner's easing functions,
    // takes the current lerp time value and interpolates it to a quartic ease out curve
    private float QuarticEaseOut(float t)
    {
        float f = (t - 1);
        return f * f * f * (1 - t) + 1;
    }
}
