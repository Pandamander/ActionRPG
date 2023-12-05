using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubzoneHarpy : SubzoneEnemy
{
    public float patrolFlipTime;
    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        patrolTime += Time.deltaTime;
        if (patrolTime >= patrolFlipTime)
        {
            patrolTime = 0f;
        }

        rigidBody.velocity = new Vector2(
            rigidBody.velocity.x,
            -moveSpeed * Time.fixedDeltaTime
        );
    }
}
