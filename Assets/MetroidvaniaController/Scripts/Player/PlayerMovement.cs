using System.Collections;
using System.Collections.Generic;
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

	//bool dashAxis = false;

	// Update is called once per frame
	void Update()
	{

		if (!canMove) { return; }

		float input = Input.GetAxisRaw("Horizontal");

        horizontalMove = input * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(input));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

		if (Input.GetButtonDown("Fire3"))
		{
			dash = true;
		}
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
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}

	public void Stop()
	{
		canMove = false;
		animator.SetBool("IsJumping", false);
		animator.SetFloat("Speed", 0f);
		animator.SetBool("IsAttacking", false);
	}

	public void AllowMovement()
	{
		canMove = true;
	}

	public void StopForDialogue()
	{
		canMove = false;
		horizontalMove = 0f;
		animator.SetBool("IsJumping", false);
		animator.SetFloat("Speed", 0f);
		animator.SetBool("IsAttacking", false);
	}
}
