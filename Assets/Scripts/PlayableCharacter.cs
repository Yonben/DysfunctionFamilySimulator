﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{

    [SerializeField] [Range(0, 100)] private int stress;
    [Range(0, 100)] private int stressMinThresholds;
    public PlayerController PlayerController;

    public void ApplyStressImpact(int stressImpact)
    {
        stress += stressImpact;
        print("ApplyStressImpact");
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        print(gameObject.name + "triggered with" + other.gameObject.name);

    }
}
