using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hungry : IndependentPenaltyBehviour
{
    [Range(1, 10)] public float maxTimeToBeHungry;
    [SerializeField] private ActionableObject _actionableObject;

    private void Awake()
    {
        base.Awake();
        HungerTimer();
    }

    public override void Off()
    {
        base.Off();
        HungerTimer();
    }

    private void HungerTimer()
    {
        Invoke("BecomeHungry", Random.Range(1f, maxTimeToBeHungry));
    }

    private void BecomeHungry()
    {
        print("Became Hungry");
        IsOn = true;
        _actionableObject.AddApplicableCharacter(player.PlayerType, this);
        StartCoroutine(nameof(TakePenalty));
    }
}
