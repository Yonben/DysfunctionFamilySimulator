using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hungry : IndependentPenaltyBehviour
{
    public float maxTimeToBeHungry;
    public float minTimeToBeHungry;
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
        Invoke("BecomeHungry", Random.Range(minTimeToBeHungry, maxTimeToBeHungry));
    }

    private void BecomeHungry()
    {
        print("Became Hungry");
        IsOn = true;
        _actionableObject.AddApplicableCharacter(player.PlayerType, this);
        StartCoroutine(nameof(TakePenalty));
    }
}
