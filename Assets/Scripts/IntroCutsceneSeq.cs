using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutsceneSeq : MonoBehaviour
{

    public CameraShake camShake;
    public ParticleSystem mastBreakParticles;
    public IntroShipMovement shipMovement;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoIntroCutsceneSeq());
    }

    private IEnumerator DoIntroCutsceneSeq()
    {

        yield return new WaitForSeconds(3.0f);

        camShake.ShakeCamera(3.0f, 2f);

        yield return null;

    }

    public void DoSerpentBiteCameraShake()
    {
        camShake.ShakeCamera(.5f, 1.5f);
    }

    public void PlayMastBreakParticles()
    {
        mastBreakParticles.Play();
    }

    public void SlowDownShip()
    {
        StartCoroutine(shipMovement.SlowDownShip(6.0f));
    }
}
