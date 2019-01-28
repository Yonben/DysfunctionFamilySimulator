using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Toilety : IndependentPenaltyBehviour
{
    public float maxTimeToBeToilety;
    public float minTimeToBeToilety;
    [SerializeField] private ActionableObject _actionableObject;

    private void Awake()
    {
        base.Awake();
        ToiletTimer();
    }

    public override void Off()
    {
        base.Off();
        ToiletTimer();
    }

    private void ToiletTimer()
    {
        Invoke("BecomeToilety", Random.Range(minTimeToBeToilety, maxTimeToBeToilety));
    }

    private void BecomeToilety()
    {

        print("Became Toilety");
        IsOn = true;

        _actionableObject.AddApplicableCharacter(player.PlayerType, this);
        StartCoroutine(nameof(TakePenalty));
    }
}
