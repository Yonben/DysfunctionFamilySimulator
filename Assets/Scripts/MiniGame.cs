using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public abstract class MiniGame : MonoBehaviour
{
    
    protected ActionableObject actionableObject;
    protected PlayableCharacter player;
    private bool isPlaying = false;

    public static Dictionary<XboxButton, string> ButtonAnimations = new Dictionary<XboxButton, string>{
        {XboxButton.A, "aButton"},
        {XboxButton.B, "bButton"},
        {XboxButton.X, "xButton"},
        {XboxButton.Y, "yButton"},
    };

    void Start()
    {
    }

    void Update()
    {
        if (isPlaying)
        {
            PlayGame();
        }
    }

    public virtual void StartMiniGame(ActionableObject actionable, PlayableCharacter initiator)
    {
        print("Start parent game");
        actionableObject = actionable;
        player = initiator;
        isPlaying = true;
    }

    public abstract void PlayGame();

    public virtual void EndMiniGame(bool miniGameSuccess = false)
    {
        // If we succeeded the mini game, end the minigame, call success on actionable and apply stress impact
        // Otherwise (like if player walks away) only end the minigame itself
        isPlaying = false;

        if (miniGameSuccess)
        {
            actionableObject.OnMiniGameSuccess(player.PlayerType);
            player.ApplyStressImpact(actionableObject.stressImpact);
        }
    }
}
