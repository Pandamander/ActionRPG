using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubzoneLevelManager : MonoBehaviour
{
    public GameObject Player;
    private Rigidbody2D rigidBody;
    private PlayerMovement movement;

    private void Awake()
    {
        movement = Player.GetComponent<PlayerMovement>();
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

            movement.SetDirection(OverworldSubzoneContainer.SubzoneLevelStartDirection);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
