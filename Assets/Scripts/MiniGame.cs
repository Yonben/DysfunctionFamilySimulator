using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public abstract class MiniGame : MonoBehaviour
{
    private Animator _animator;
    public bool desapireInEnter;
    public string objectAnimationEnter;
    public string objectAnimationExit;
    public string playerAnimationEnter;
    public string playerAnimationExit;
    public bool objectAnimationEntered;
    public bool playerAnimationEntered;
    
    protected ActionableObject actionableObject;
    public PlayableCharacter player;
    private bool isPlaying = false;

    [HideInInspector] public bool inMiniGame = false;

    public static Dictionary<XboxButton, string> ButtonAnimations = new Dictionary<XboxButton, string>{
        {XboxButton.A, "aButton"},
        {XboxButton.B, "bButton"},
        {XboxButton.X, "xButton"},
        {XboxButton.Y, "yButton"},
    };
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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
        inMiniGame = true;
        print("Start parent game");
        actionableObject = actionable;
        player = initiator;
        isPlaying = true;
        
        if (!objectAnimationEnter.Equals("") && !objectAnimationEntered)
        {
            objectAnimationEntered = true;
            _animator.SetTrigger(objectAnimationEnter);
        }
                
        if (!playerAnimationEnter.Equals("") && !playerAnimationEntered)
        {
            playerAnimationEntered = true;
            player.m_animator.SetTrigger(playerAnimationEnter);
        }

        if (desapireInEnter)
        {
            player.enabledMovement(false);
            player.spriteRenderer.enabled = false;
        }
    }

    public abstract void PlayGame();

    public virtual void EndMiniGame(bool miniGameSuccess = false, bool triggerExit = false)
    {
        

        

        inMiniGame = false;
        
        // If we succeeded the mini game, end the minigame, call success on actionable and apply stress impact
        // Otherwise (like if player walks away) only end the minigame itself
        isPlaying = false;
        
        if (!objectAnimationExit.Equals("") && objectAnimationEntered)
        {
            _animator.SetTrigger(objectAnimationExit);
            objectAnimationEntered = false;
        }
                
        

        if (player)
        {
            if (!playerAnimationExit.Equals("") && playerAnimationEntered)
            {
                player.m_animator.SetTrigger(playerAnimationExit);
                playerAnimationEntered = false;
            }
            
            
            if (desapireInEnter)
            {
                player.enabledMovement(true);
                player.spriteRenderer.enabled = true;
            }

            if (!triggerExit)
            {
                if (actionableObject.IsBroken)
                {
                    if (actionableObject._brokenMiniGame.CharExitMiniGamePos)
                        player.transform.position = actionableObject._brokenMiniGame.CharExitMiniGamePos.transform.position;
                }
                else
                {
                    
                        player.transform.position = actionableObject.CharExitMiniGamePos.transform.position;
                }
                
            }
        }
        
        
        if (actionableObject)
        {
            actionableObject.OnMiniGameEnd(player, miniGameSuccess);
        }

        player = null;
    }
}
