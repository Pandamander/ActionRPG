using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubzoneAudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip attack;
    public AudioClip damage;
    // Start is called before the first frame update

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
}
