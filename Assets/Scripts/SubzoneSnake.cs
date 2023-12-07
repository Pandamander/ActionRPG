using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubzoneSnake : SubzoneEnemy
{
    public float patrolFlipTime;
    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (_isDying) { return; }

        patrolTime += Time.deltaTime;
        if (patrolTime >= patrolFlipTime)
        {
            Flip();
            patrolTime = 0f;
        }

        rigidBody.velocity = new Vector2(
            -moveSpeed * Time.fixedDeltaTime,
            rigidBody.velocity.y
        );
    }
}
