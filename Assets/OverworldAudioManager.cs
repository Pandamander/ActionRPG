using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldAudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip enemyEncounter;
    //public AudioClip entrance;

    public void PlayEnemyEncounter()
    {
        source.clip = enemyEncounter;
        source.Play();
    }

/*    public void PlayEntrance()
    {
        source.clip = entrance;
        source.Play();
    }*/
}
