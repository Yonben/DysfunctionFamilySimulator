using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveAura : MonoBehaviour
{
    [SerializeField] private int stressToReduce;

    [SerializeField] private float timeToEachReducedStress;
    
    private List<PlayableCharacter> players = new List<PlayableCharacter>();


    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayableCharacter player = other.GetComponent<PlayableCharacter>();
        if (!player)
            return;

        players.Add(player);
        StartCoroutine(nameof(ReduceStress), player);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayableCharacter player = other.GetComponent<PlayableCharacter>();
        if (!player)
            return;
        players.Remove(player);
    }

    IEnumerator ReduceStress(PlayableCharacter player)
    {
        yield return new WaitForSeconds(timeToEachReducedStress);
        while (players.Contains(player))
        {
            player.ApplyStressImpact(stressToReduce);
            yield return new WaitForSeconds(timeToEachReducedStress);
        }
    }
}
