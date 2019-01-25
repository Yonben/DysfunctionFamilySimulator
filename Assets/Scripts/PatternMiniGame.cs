using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternMiniGame : MiniGame
{
    public enum buttons { A, B, C, D };
    public List<buttons> buttonsPattern;
    private buttons buttonToPress;
    private int currentKeyToPress;

    void Start()
    {

    }

    public override void PlayGame()
    {
        if (true)
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
