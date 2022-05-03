using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireTrailRendererBehavior : MonoBehaviour
{
    PlayerMovement playerMovement;
    TrailRenderer trailRenderer;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.TireSkid(out float lateralVelocity, out bool isBraking))
        {
            AudioManager.Instance.PlaySkid();
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
