using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IndependentPenaltyBehviour : MonoBehaviour
{
    protected PlayableCharacter player;
    
    [SerializeField] private int penalty;
    [SerializeField] private float penaltyTimeRate;

    protected bool IsOn = false;

    protected virtual void Awake()
    {
        player = GetComponent<PlayableCharacter>();
    }

    protected IEnumerator TakePenalty()
    {
        print("TakePenalty");
        while (IsOn)
        {
            player.ApplyStressImpact(penalty);
            yield return new WaitForSeconds(penaltyTimeRate);
        }
    }

    public void Off()
    {
        IsOn = false;
        print("Penalty Off");
    }
}
