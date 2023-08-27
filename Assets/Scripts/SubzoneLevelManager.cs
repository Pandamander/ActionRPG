using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubzoneLevelManager : MonoBehaviour
{
    public GameObject Player;
    private PlayerMovement Movement;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private void Awake()
    {
        Movement = Player.GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        if (OverworldSubzoneContainer.UseSubzoneLevelStartPosition)
        {
            OverworldSubzoneContainer.UseSubzoneLevelStartPosition = false;

            rigidBody = Player.GetComponent<Rigidbody2D>();
            rigidBody.transform.position = new Vector3(
                OverworldSubzoneContainer.SubzoneLevelStartPosition.Item1,
                OverworldSubzoneContainer.SubzoneLevelStartPosition.Item2
            );

            animator = Movement.GetComponent<Animator>();

            switch (OverworldSubzoneContainer.LastEncounterDirection)
            {
                case OverworldSubzoneContainer.PlayerDirection.Up:
                    break;
                case OverworldSubzoneContainer.PlayerDirection.Down:
                    break;
                case OverworldSubzoneContainer.PlayerDirection.Left:
                    animator.SetFloat("speed", -1);
                    break;
                case OverworldSubzoneContainer.PlayerDirection.Right:
                    animator.SetFloat("speed", 1);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
