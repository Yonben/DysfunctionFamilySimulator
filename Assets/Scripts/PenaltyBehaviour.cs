using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenaltyBehaviour : MonoBehaviour
{
    public List<PlayableCharacter> players;

    [SerializeField] private int penalty;
    [SerializeField] private float penaltyTimeRate;


    [SerializeField] private Sprite needIcon;



    private bool isOn = false;

    public bool IsOn {
        get
        {
            return isOn;
        }
        set
        {
            isOn = value;
            foreach (PlayableCharacter player in players)
            {
                if (isOn)
                {
                    if (!player.needsSprites.Contains(needIcon))
                    {
                        player.needsSprites.Add(needIcon);
                    }
                }
                else
                {
                    player.needsSprites.Remove(needIcon);
                }
            }

        }
    }

    protected IEnumerator TakePenalty()
    {
        print("TakePenalty");
        while (IsOn)
        {
            foreach (PlayableCharacter player in players)
            {
                player.ApplyStressImpact(penalty);
            }
            yield return new WaitForSeconds(penaltyTimeRate);
        }
    }

    public virtual void Off()
    {
        IsOn = false;
        print("Penalty Off");
    }
}
