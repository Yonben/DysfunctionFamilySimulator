using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IndependentPenaltyBehviour : MonoBehaviour
{
    protected PlayableCharacter player;

    [SerializeField] private int penalty;
    [SerializeField] private float penaltyTimeRate;



    private bool isOn = false;

    public bool IsOn {
        get
        {
            return isOn;
        }
        set
        {
            isOn = value;

        }
    }

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

    virtual public void Off()
    {
        IsOn = false;
        print("Penalty Off");
    }
}
