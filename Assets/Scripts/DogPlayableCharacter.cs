using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlayableCharacter : PlayableCharacter
{
	[SerializeField] private GameObject poop;

	[SerializeField] private float poopIntervalTime;

	[SerializeField] private Transform poopTransform;

	private Transform _transform;

	[SerializeField] private int stressToShowNeed = 80;
	[SerializeField] private Sprite poopNeedIcon;

	protected override void Awake()
	{
		base.Awake();
		_transform = transform;
	}

	protected override void Start()
	{
		base.Start();
		StartCoroutine(nameof(defecateTimer));
	}

	IEnumerator defecateTimer()
	{
		while (true)
		{
			yield return new WaitForSeconds(poopIntervalTime);
			ApplyStressImpact(1);
			if (stress >= stressToShowNeed)
			{
//				print("need!");
				if (!needsSprites.Contains(poopNeedIcon))
				{
					needsSprites.Add(poopNeedIcon);
				}
			}
		}
	}

	private void defecate()
	{
		Vector3 position = _transform.position;
		position += PlayerController.isRight ? poopTransform.localPosition : -poopTransform.localPosition;
		Instantiate(poop, position, Quaternion.identity);
		
		needsSprites.Remove(poopNeedIcon);
	}
	
	protected override void Die()
	{
		defecate();
		stress = 0;
	}
}
