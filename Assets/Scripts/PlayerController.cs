using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


public class PlayerController : MonoBehaviour
{
	public XboxController controller;

	[SerializeField] private float maxMovementVelocity;
	[SerializeField] private float movementForce;

	private Rigidbody2D m_Rigidbody2D;
	private Transform _transform;
	private float movementHorizontal, movementVertical;

	internal bool canMove = true;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		_transform = transform;
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
		movementHorizontal = axisX;
		movementVertical = axisY;

		float targetHorizontalVelocity = movementHorizontal * maxMovementVelocity;
		if (targetHorizontalVelocity != 0)
			_transform.localScale = new Vector3(targetHorizontalVelocity > 0 ? 1 : -1, 1, 1);
	}
}
