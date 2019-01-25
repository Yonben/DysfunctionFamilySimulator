using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PatternMiniGame : MiniGame
{
    public List<XboxButton> buttonsPattern;

    private int currentButtonIndex;
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
        buttonToPress = buttonsPattern[currentButtonIndex];
        PatternButtonAnim.SetTrigger(MiniGame.ButtonAnimations[buttonToPress]);
    }

    public override void EndMiniGame(bool miniGameSuccess = false)
    {
        base.EndMiniGame(miniGameSuccess);

        currentButtonIndex = 0;
        if (buttonInstance)
        {
            Destroy(buttonInstance);
        }
    }

    public override void PlayGame()
    {
        if (XCI.GetButtonDown(buttonToPress))
        { // Check if the currentKeyToPress is pressed
            if (currentButtonIndex == buttonsPattern.Count - 1)
            {
                currentButtonIndex = 0;
                EndMiniGame(miniGameSuccess: true);
                return;
            }
            currentButtonIndex++;
            GetNextButtonToPress();

        }
    }
}
