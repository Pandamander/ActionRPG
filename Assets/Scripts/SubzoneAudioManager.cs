using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubzoneAudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioSource backgroundSource;
    public AudioClip attack;
    public AudioClip damage;
    public AudioClip powerup;
    public AudioClip gameOver;
    public AudioClip backgroundMusic;
    public AudioClip arcadeJump;
    public AudioClip attackHit;

    public void StopMusic()
    {
        backgroundSource.Stop();
    }
    public void PlayAttack()
    {
        source.clip = attack;
        source.Play();
    }

    public void PlayAttackHit()
    {
        source.clip = attackHit;
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

    public void PlayArcadeJump()
    {
        source.clip = arcadeJump;
        source.Play();
    }

    public void PlayGameOver()
    {
        source.clip = gameOver;
        source.Play();
    }

    // Play background music on start
    public void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        // If background music clip is not null, play it
        if (backgroundMusic != null)
        {
            backgroundSource.clip = backgroundMusic;
            backgroundSource.Play();
        }
        
    }
}
