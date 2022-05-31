using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubzoneAudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip attack;
    public AudioClip damage;
    public AudioClip powerup;

    public void PlayAttack()
    {
        source.clip = attack;
        source.Play();
    }

    public void PlayDamage()
    {
        source.clip = damage;
        source.Play();
    }

    public void PlayPowerup()
    {
        source.clip = powerup;
        source.Play();
    }
}
