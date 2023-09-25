using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine;

public class CyclopsBossFight : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private BoxCollider2D[] colliders;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform virtualCameraBossFightTarget;
    [SerializeField] private CyclopsBossStateMachine stateMachine;

    private bool moveCam = false;
    private bool playerNeedsUnfreeze = false;

    private void Awake()
    {
        DisableColliders();
    }

    private void DisableColliders()
    {
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void EnableColliders()
    {
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNeedsUnfreeze)
        {
            playerNeedsUnfreeze = false;
            EnableColliders();
            playerMovement.AllowMovement();
        }
        if (moveCam)
        {
            virtualCamera.transform.position = Vector3.MoveTowards(
                virtualCamera.transform.position,
                virtualCameraBossFightTarget.position,
                Time.deltaTime * 7f
            );
            
            if (virtualCamera.transform.position.x >= virtualCameraBossFightTarget.position.x)
            {
                moveCam = false;
                playerNeedsUnfreeze = true;
                stateMachine.Run();
            }
        }
    }

    private void RetargetVirtualCamera()
    {
        virtualCamera.Follow = null;
        moveCam = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            playerMovement.FreezeWalking();
            RetargetVirtualCamera();
        }
    }
}
