using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.U2D;

public class BossFightManager : MonoBehaviour
{
    public int bossHealth;
    public float sinusoidalMoveSpeed;
    public float swoopAttackMoveSpeed;
    private BossStateMachine stateMachine;
    public CinemachineVirtualCamera vCam;
    private CinemachineTransposer transposer;
    public Camera mainCam;
    private PixelPerfectCamera ppCam;
    private float yOffset = 6f;
    private bool shouldMoveCamera = false;
    private float moveCameraTimer = 0f;

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
            moveCameraTimer += Time.deltaTime;
            if (moveCameraTimer >= 2f)
            {
                shouldMoveCamera = false;
            } else
            {
                transposer.m_FollowOffset.y += (Time.deltaTime + 0.004f);
                //Debug.Log("transposer.m_FollowOffset.y: " + transposer.m_FollowOffset.y);
            }
        }
    }

    private IEnumerator StartBossFight()
    {
        stateMachine.TransitionTo(BossStateMachine.BossState.Intro);
        yield return new WaitForSeconds(2.0f);
        stateMachine.TransitionTo(BossStateMachine.BossState.Sin);
    }

    public void BeginBossFight()
    {
        //ppCam.assetsPPU = 8;

        //StartCoroutine(StartBossFight());
        shouldMoveCamera = true;
        stateMachine.TransitionTo(BossStateMachine.BossState.Sin);
    }
}
