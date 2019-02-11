using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyCry : PenaltyBehaviour
{
	[SerializeField] private PlayableCharacter player;
	[SerializeField] private float maxTimeToCry;
	[SerializeField] private float minTimeToCry;
	[SerializeField] private ActionableObject _actionableObject;
    
	protected virtual void Awake()
	{	
		CryTimer();
	}

	public override void Off()
	{
		base.Off();
		CryTimer();
	}

	private void CryTimer()
	{
		Invoke("BabyStartToCry", Random.Range(minTimeToCry, maxTimeToCry));
	}

	private void BabyStartToCry()
	{

		print("Baby Cry");
		IsOn = true;

		_actionableObject.AddApplicableCharacter(player.PlayerType, this);
		StartCoroutine(nameof(TakePenalty));
	}
}
