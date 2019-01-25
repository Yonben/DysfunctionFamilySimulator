using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PatternMiniGame : MiniGame
{
    public List<XboxButton> buttonsPattern;
    private XboxButton buttonToPress = XboxButton.B;

    void Start()
    {
        GetNextButtonToPress();
        print("Button to press: " + buttonToPress);
    }

    private void GetNextButtonToPress()
    {
        buttonToPress = buttonsPattern[0];
        buttonsPattern.RemoveAt(0);
    }

    public override void PlayGame()
    {
        if (XCI.GetButtonDown(buttonToPress))
        { // Check if the currentKeyToPress is pressed
            if (buttonsPattern.Count == 0)
            {
                EndMiniGame(miniGameSuccess: true);
                return;
            }
            GetNextButtonToPress();
            print("Button to press: " + buttonToPress);

        }
    }
}
