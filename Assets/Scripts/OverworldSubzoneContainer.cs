using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldSubzoneContainer
{
    public static (float, float) LastEncounterPosition { get; private set; }
    public static string LastEncounterSubzoneName { get; private set; }
    public static string LastEncounterEnemyName { get; private set; }

    private static bool Initialized;

    public static void AddEncounter(float x, float y, string subzoneName, string enemyName)
    {
        LastEncounterPosition = (x, y);
        LastEncounterSubzoneName = subzoneName;
        LastEncounterEnemyName = enemyName;
    }

    public static void Initialize()
    {
        if (!Initialized)
        {
            LastEncounterPosition = (5.5f, -2.5f);
            LastEncounterSubzoneName = "";
            LastEncounterEnemyName = "";
            Initialized = true;
        }
    }
}