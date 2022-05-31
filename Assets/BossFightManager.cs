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
    public float sinPhaseTime;
    public float swoopPhaseTime;
    private BossStateMachine stateMachine;
    public CinemachineVirtualCamera vCam;
    private CinemachineTransposer transposer;
    public Camera mainCam;
    private PixelPerfectCamera ppCam;
    private bool shouldMoveCamera = false;
    private bool playedIntro = false;
    private float bossStateTimer = 0f;
    private bool shouldStartBossTimer = false;

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

        if (shouldStartBossTimer)
        {
            bossStateTimer += Time.deltaTime;
            switch (stateMachine.currentlyRunningState)
            {
                case BossStateMachine.BossState.Sin:
                    if (bossStateTimer >= sinPhaseTime)
                    {
                        bossStateTimer = 0;
                        stateMachine.TransitionTo(BossStateMachine.BossState.Swoop);
                    }
                    break;
                case BossStateMachine.BossState.Swoop:
                    if (bossStateTimer >= swoopPhaseTime)
                    {
                        bossStateTimer = 0;
                        stateMachine.TransitionTo(BossStateMachine.BossState.Sin);
                    }
                    break;
            }
        }
    }

    private IEnumerator StartBossFight()
    {
        yield return new WaitForSeconds(1f);
        stateMachine.Run();
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
            shouldStartBossTimer = false;
            stateMachine.BossDead();
        }
    }

    public void InitialBossPhaseDidBegin()
    {
        shouldStartBossTimer = true;
    }
}
