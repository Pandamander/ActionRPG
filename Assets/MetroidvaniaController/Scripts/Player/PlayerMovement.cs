using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;
	public Animator animator;

	[SerializeField] private float runSpeed = 40f;

	public float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;
	public bool canMove = true;
	public bool grounded { get; private set; }
	private bool StopFixedUpdate = false;
    private Attack attack;
	public bool isAttacking
	{
		get
		{
			return attack.isAttacking;
		}
	}
	public bool isDamaged
    {
        get
        {
            return attack.isDamaged;
        }
    }

    //bool dashAxis = false;

    private void Awake()
    {
        attack = GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
	{

		if (!canMove) { return; }

		float inputHorizontal = Input.GetAxisRaw("Horizontal");

        horizontalMove = inputHorizontal * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(inputHorizontal));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

		if (Input.GetButtonDown("Fire3"))
		{
			dash = true;
		}

        grounded = controller.m_Grounded;

        float inputVertical = Input.GetAxisRaw("Vertical");

        if (grounded)
		{
			if (inputVertical == -1f)
			{
                Crouch();
            } else
			{
				UnCrouch();
			}
		}
	}

	private void Crouch()
	{
        horizontalMove = 0f;
        animator.SetBool("IsCrouching", true);
    }

    private void UnCrouch()
    {
        animator.SetBool("IsCrouching", false);
    }

    public void DoJump()
	{
		jump = true;
	}

	public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate()
	{
		if (StopFixedUpdate) { return; } 
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}

	public void Stop()
	{
		horizontalMove = 0f;
        canMove = false;
		StopFixedUpdate = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		animator.SetBool("IsJumping", false);
		animator.SetFloat("Speed", 0f);
		animator.SetBool("IsAttacking", false);
	}

	public void StopForAttack()
	{
        horizontalMove = 0f;
        canMove = false;
        StopFixedUpdate = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void StopForKnockback()
    {
        horizontalMove = 0f;
        canMove = false;
        StopFixedUpdate = true;
        animator.SetBool("IsJumping", false);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("IsAttacking", false);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void StopAirControlForJumpAttack()
    {
        canMove = false;
    }

    public void AllowMovement()
	{
		canMove = true;
        StopFixedUpdate = false;
    }

	public void StopForDialogue()
	{
		canMove = false;
		horizontalMove = 0f;
		animator.SetBool("IsJumping", false);
		animator.SetFloat("Speed", 0f);
		animator.SetBool("IsAttacking", false);
	}

	public void FreezeWalking()
	{
        canMove = false;
        horizontalMove = 0f;
        animator.SetBool("IsJumping", false);
        animator.SetFloat("Speed", 1f);
        animator.SetBool("IsAttacking", false);
    }

	public void SetDirection(OverworldSubzoneContainer.PlayerDirection direction)
	{
        switch (direction)
        {
            case OverworldSubzoneContainer.PlayerDirection.Up:
                break;
            case OverworldSubzoneContainer.PlayerDirection.Down:
                break;
            case OverworldSubzoneContainer.PlayerDirection.Left:
				controller.Flip();
                break;
            case OverworldSubzoneContainer.PlayerDirection.Right:
                break;
        }
    }
}
