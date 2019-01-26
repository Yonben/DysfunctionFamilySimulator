using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Smelly : IndependentPenaltyBehviour
{
	[SerializeField] private float distanceToBeSmelly;
	[SerializeField] private ActionableObject _actionableObject;

	private Transform _transform;

	private float distance;
	private Vector2 lastPosition;

	private void Awake()
	{
		base.Awake();
		_transform = transform;
		distance = 0;
		lastPosition = _transform.position;
	}

	private void FixedUpdate()
	{
		Vector2 position = _transform.position;
		if (!IsOn)
		{
			distance += Vector2.Distance(position, lastPosition);

			if (distance >= distanceToBeSmelly)
			{
				distance = 0;
				IsOn = true;
				_actionableObject.AddApplicableCharacter(player.PlayerType, this);
				StartCoroutine(nameof(TakePenalty));
			}
		}
		lastPosition = position;
	}
}
