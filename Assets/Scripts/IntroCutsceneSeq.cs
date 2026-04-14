using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutsceneSeq : MonoBehaviour
{

    public CameraShake camShake;
    public ParticleSystem mastBreakParticles;
    public ParticleSystem shipParticles;
    public IntroShipMovement shipMovement;
    public Animator seaSerpentBossAnimator;
    public SeaSerpentBossController bossController;

    private bool hasSerpentAppeared = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoIntroCutsceneSeq());
    }

    void Update()
    {
        // when the ship comes to a complete stop, do another camera shake and play the serpeant appearance animation
        if(shipMovement.speed == 0)
        {
            if (!hasSerpentAppeared) {
                // camShake.ShakeCamera(3.0f, 2f);
                hasSerpentAppeared = true;
                // TODO: Elliott make serpent boss appear and start boss fight
                seaSerpentBossAnimator.SetTrigger("Appear");
                StartCoroutine(StartBossFight());
            }
        }
    }

    private IEnumerator DoIntroCutsceneSeq()
    {

        yield return new WaitForSeconds(3.0f);

        camShake.ShakeCamera(3.0f, 2f);

        yield return null;

    }

    private IEnumerator StartBossFight()
    {
        yield return new WaitForSeconds(5.0f);

        bossController.StartBoss();

        yield return null;
    }

    public void DoSerpentBiteCameraShake()
    {
        camShake.ShakeCamera(.5f, 1.7f);
    }

    public void PlayMastBreakParticles()
    {
        mastBreakParticles.Play();
    }

    public void SlowDownShip()
    {
        StartCoroutine(shipMovement.SlowDownShip(6.0f));
        // also stop water particles here
        shipParticles.GetComponent<IntroShipMovement>().speed = 0;
        shipParticles.Stop();
    }
}
