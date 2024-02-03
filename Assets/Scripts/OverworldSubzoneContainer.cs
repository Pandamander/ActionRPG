using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldSubzoneContainer
{
    public enum PlayerDirection { Up, Down, Left, Right }
    public static (float, float) LastEncounterPosition { get; private set; }
    public static PlayerDirection LastEncounterDirection { get; private set; }
    public static string LastEncounterSubzoneName { get; private set; }

    public static (float, float) SubzoneLevelStartPosition { get; private set; }
    public static PlayerDirection SubzoneLevelStartDirection { get; private set; }
    public static bool UseSubzoneLevelStartPosition { get; set; }

    public static bool HasShownWreckedShipIntro { get; set; }

    private static bool Initialized;

    public static void AddEncounter(float x, float y, string subzoneName, PlayerDirection direction)
    {
        LastEncounterPosition = (x, y);
        LastEncounterSubzoneName = subzoneName;
        LastEncounterDirection = direction;
    }

    public static void AddSubzoneStartPosition(float x, float y, PlayerDirection direction)
    {
        SubzoneLevelStartPosition = (x, y);
        SubzoneLevelStartDirection = direction;
        UseSubzoneLevelStartPosition = true;
    }

    public static void Initialize()
    {
        if (!Initialized)
        {
            LastEncounterPosition = (6.3f, -2.8f); // Start near wrecked ship (default)
            // LastEncounterPosition = (21.54f, 16.3f); // Start near Intro Forest South
            // LastEncounterPosition = (16.5f, 26f); // Start near Intro Forest North
            // LastEncounterPosition = (16.5f, 32.5f); // Start near Boat
            LastEncounterSubzoneName = "";
            LastEncounterDirection = PlayerDirection.Down;

            Initialized = true;
        }
    }

    public static void Reset()
    {
        Initialized = false;
    }
}
