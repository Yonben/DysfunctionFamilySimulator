using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IndependentPenaltyBehviour : PenaltyBehaviour
{
    protected PlayableCharacter player;
    
    protected virtual void Awake()
    {
        player = GetComponent<PlayableCharacter>();
        if (!players.Contains(player))
            players.Add(player);
    }
}
