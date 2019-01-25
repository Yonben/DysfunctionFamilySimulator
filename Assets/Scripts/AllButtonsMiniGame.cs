using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllButtonsMiniGame : MiniGame
{
    public enum buttons { A, B, C, D };
    public List<buttons> pressableButtons = new List<buttons> { buttons.A, buttons.B, buttons.C, buttons.D };
    public int numberOfTap = 25;

    void Start()
    {

    }

    public override void PlayGame()
    {
        foreach (var button in pressableButtons)
        {
            // Check if pressed
            if (true)
            {
                numberOfTap--; // Or -4?
                if (numberOfTap <= 0)
                {
                    EndMiniGame();
                }
            }
        }
    }
}
