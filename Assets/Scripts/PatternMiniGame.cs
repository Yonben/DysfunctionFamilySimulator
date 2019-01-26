﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PatternMiniGame : MiniGame
{
    public List<XboxButton> buttonsPattern;
    public bool isRandom = false;
    public int stepsNumber = 0;
    public int infiniteStepsNumber = 5;

    private List<XboxButton> possibleButtons = new List<XboxButton> { XboxButton.A, XboxButton.B, XboxButton.X, XboxButton.Y };
    private int currentButtonIndex;
    private int infiniteStepsCount = 0;

    private XboxButton buttonToPress;
    private GameObject buttonInstance;
    private Animator PatternButtonAnim;

    [SerializeField] private Transform t;

    public override void StartMiniGame(ActionableObject actionable, PlayableCharacter initiator)
    {
        base.StartMiniGame(actionable, initiator);

        var actionableButtonPos = actionableObject.PatternButtonPos.position;
        buttonInstance = (GameObject)Instantiate(PatternButton, actionableButtonPos, Quaternion.identity);
        PatternButtonAnim = buttonInstance.GetComponent<Animator>();

        GetNextButtonToPress();
    }

    private void GetNextButtonToPress()
    {
        if (isRandom)
        {
            buttonToPress = possibleButtons[Random.Range(0, possibleButtons.Count)];
        }
        else
        {
            buttonToPress = buttonsPattern[currentButtonIndex];
        }
        PatternButtonAnim.SetTrigger(MiniGame.ButtonAnimations[buttonToPress]);
    }

    public override void EndMiniGame(bool miniGameSuccess = false)
    {
        base.EndMiniGame(miniGameSuccess);

        currentButtonIndex = 0;
        infiniteStepsCount = 0;
        if (buttonInstance)
        {
            Destroy(buttonInstance);
        }
    }

    public override void PlayGame()
    {
        if (XCI.GetButtonDown(buttonToPress))
        {
            if (stepsNumber == 0 && infiniteStepsNumber > 0)
            {
                if (++infiniteStepsCount >= infiniteStepsNumber)
                {
                    player.ApplyStressImpact(actionableObject.stressImpact);
                    infiniteStepsCount = 0;
                }
            }
            else if (isRandom && stepsNumber > 0)
            {
                if (++infiniteStepsCount >= stepsNumber)
                {
                    EndMiniGame(miniGameSuccess: true);
                    return;
                }
            }
            else
            {
                if (currentButtonIndex == buttonsPattern.Count - 1)
                {
                    currentButtonIndex = 0;
                    EndMiniGame(miniGameSuccess: true);
                    return;
                }
                currentButtonIndex++;
            }

            GetNextButtonToPress();

        }
    }
}
