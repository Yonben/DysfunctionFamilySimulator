using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PatternMiniGame : MiniGame
{
    public List<XboxButton> buttonsPattern;
    private XboxButton buttonToPress;

    public override void PlayGame()
    {
        if (XCI.GetButtonDown(buttonToPress))
        { // Check if the currentKeyToPress is pressed
            if (buttonsPattern.Count == 0)
            {
                EndMiniGame();
                return;
            }
            buttonToPress = buttonsPattern[0];
            buttonsPattern.RemoveAt(0);
        }
    }
}
