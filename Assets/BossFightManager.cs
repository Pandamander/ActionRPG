using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.U2D;

public class BossFightManager : MonoBehaviour
{
    public float bossHealth;
    public float sinusoidalMoveSpeed;
    public float swoopAttackMoveSpeed;
    private BossStateMachine stateMachine;
    public CinemachineVirtualCamera vCam;
    private CinemachineTransposer transposer;
    public Camera mainCam;
    private PixelPerfectCamera ppCam;
    private bool shouldMoveCamera = false;
    private bool playedIntro = false;

    private void Awake()
    {
        stateMachine = gameObject.GetComponent<BossStateMachine>();
        CinemachineComponentBase componentBase = vCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        transposer = (componentBase as CinemachineTransposer);
        //ppCam = mainCam.GetComponentInParent<PixelPerfectCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        stateMachine.UpdateMovementSpeeds(sinusoidalMoveSpeed, swoopAttackMoveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMoveCamera)
        {
            if (transposer.m_FollowOffset.y >= 6f)
            {
                shouldMoveCamera = false;
            } else
            {
                transposer.m_FollowOffset.y += (Time.deltaTime + 0.008f);
            }
        }
    }

    private IEnumerator StartBossFight()
    {
        yield return new WaitForSeconds(1f);
        stateMachine.TransitionTo(BossStateMachine.BossState.Sin);
    }

    public void BeginBossFight()
    {
        if (playedIntro) { return; }

        playedIntro = true;
        shouldMoveCamera = true;
        StartCoroutine(StartBossFight());
    }

    public void ApplyDamage(float damage)
    {
        bossHealth -= damage;
        if (bossHealth <= 0f)
        {

        }
    }
}
