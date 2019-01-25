using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MiniGame : MonoBehaviour
{
    private ActionableObject actionableObject;
    private PlayableCharacter player;
    private bool isPlaying = false;

    void Update()
    {
        if (isPlaying)
        {
            PlayGame();
        }
    }

    public void StartMiniGame(ActionableObject actionable, PlayableCharacter initiator)
    {
        actionableObject = actionable;
        player = initiator;
        // Do Game
        EndMiniGame();
    }

    public abstract void PlayGame();

    protected void EndMiniGame()
    {
        // If we succeeded the mini game, end the minigame, call success on actionable and apply stress impact
        // Otherwise (like if player walks away) only end the minigame itself
        if (true)
        {
            actionableObject.OnMiniGameSuccess();
            player.ApplyStressImpact(actionableObject.stressImpact);
        }
    }
}
