using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public abstract class MiniGame : MonoBehaviour
{
    private ActionableObject actionableObject;
    private PlayableCharacter player;
    private bool isPlaying = false;
    static Dictionary<XboxButton, string> ButtonAnimations = new Dictionary<XboxButton, string>{
        {XboxButton.A, "aButton"},
        {XboxButton.B, "bButton"},
        {XboxButton.X, "xButton"},
        {XboxButton.Y, "yButton"},
    };

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
        isPlaying = true;
        // EndMiniGame();
    }

    public abstract void PlayGame();

    protected void EndMiniGame(bool miniGameSuccess = false)
    {
        // If we succeeded the mini game, end the minigame, call success on actionable and apply stress impact
        // Otherwise (like if player walks away) only end the minigame itself
        isPlaying = false;
        if (miniGameSuccess)
        {
            actionableObject.OnMiniGameSuccess();
            player.ApplyStressImpact(actionableObject.stressImpact);
        }
    }
}
