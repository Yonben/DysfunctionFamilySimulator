using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


public class PlayerController : MonoBehaviour
{
	private const float JOYSTICK_THRESHOLD = 0.1f;
	
	public XboxController controller;

	[SerializeField] private float maxMovementVelocity;
	[SerializeField] private float movementForce;

	private Rigidbody2D m_Rigidbody2D;
	private SpriteRenderer _spriteRenderer;
	private float movementHorizontal, movementVertical;

	internal bool canMove = true;

	public bool isRight = true;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void FixedUpdate()
	{
		if (canMove)
			move();
	}

	private void move()
	{
		float targetHorizontalVelocity = movementHorizontal * maxMovementVelocity;
		float differenceHorizontal = targetHorizontalVelocity - m_Rigidbody2D.velocity.x;
		float normalizedHorizontalDifference = differenceHorizontal / maxMovementVelocity;
		
		float targetVerticalVelocity = movementVertical * maxMovementVelocity;
		float differenceVertical = targetVerticalVelocity - m_Rigidbody2D.velocity.y;
		float normalizedVerticalDifference = differenceVertical / maxMovementVelocity;

		Vector2 forceToAdd = new Vector2(normalizedHorizontalDifference, normalizedVerticalDifference) * movementForce;
//		print(forceToAdd);
		m_Rigidbody2D.AddForce(forceToAdd);
	}

	private void Update()
	{
		float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
		float axisY = XCI.GetAxis(XboxAxis.LeftStickY, controller);
		if (Math.Abs(axisX) < JOYSTICK_THRESHOLD)
		{
			axisX = 0;
		}
		if (Math.Abs(axisY) < JOYSTICK_THRESHOLD)
		{
			axisY = 0;
		}
		movementHorizontal = axisX;
		movementVertical = axisY;

		float targetHorizontalVelocity = movementHorizontal * maxMovementVelocity;
		if (targetHorizontalVelocity != 0)
		{
			isRight = targetHorizontalVelocity > 0;
			_spriteRenderer.flipX = !isRight;
		}
	}

	public bool isTryMoove()
	{
		return Math.Abs(movementHorizontal) > JOYSTICK_THRESHOLD || Math.Abs(movementVertical) > JOYSTICK_THRESHOLD;
	}
}
