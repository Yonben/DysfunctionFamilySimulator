﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class ActionableObject : MonoBehaviour
{

    
    
    public bool isExplicit;
    public int stressImpact;
    public bool hasGlobalImpact;
    public Transform PatternButtonPos;
    [HideInInspector] public GameObject buttonInstance;
    [HideInInspector] public Animator PatternButtonAnim;
    public GameObject PatternButton;
    
    public Transform CharPos;
    public bool CharFacingRight = true;
    public List<GameManager.PlayerType> ApplicableCharacters;

    public Sprite idleState;
    public Sprite needsActionState;

    public MiniGame MiniGameScript;

    private PlayableCharacter playableCharacter;

    private bool isBroken;

    [SerializeField] private bool removeAllPlayer;

    private Dictionary<GameManager.PlayerType, IndependentPenaltyBehviour> _independentPenaltyBehvioursMap =
        new Dictionary<GameManager.PlayerType, IndependentPenaltyBehviour>();
//    private IndependentPenaltyBehviour _independentPenaltyBehviour;


    

    // Start is called before the first frame update
    void Start()
    {
        if (!buttonInstance)
            buttonInstance = (GameObject)Instantiate(PatternButton, PatternButtonPos.position, Quaternion.identity);
        PatternButtonAnim = buttonInstance.GetComponent<Animator>();
        buttonInstance.SetActive(false);
    }

    public void OnMiniGameSuccess(GameManager.PlayerType playerType)
    {
        // Reset Actionable state
        print("OnMiniGameSuccess");
//        _independentPenaltyBehviour.Off();
        
        ApplicableCharacters.Remove(playerType);
        if (removeAllPlayer)
        {
            foreach (var key in _independentPenaltyBehvioursMap.Keys)
            {
                _independentPenaltyBehvioursMap[key].Off();
                _independentPenaltyBehvioursMap.Remove(key);
            }
        }
        else
        {
            _independentPenaltyBehvioursMap[playerType].Off();
            _independentPenaltyBehvioursMap.Remove(playerType);
        }
    }

    public void AddApplicableCharacter(GameManager.PlayerType playerType)
    {
        ApplicableCharacters.Add(playerType);
    }

    public void AddApplicableCharacter(GameManager.PlayerType playerType, IndependentPenaltyBehviour independentPenaltyBehviour)
    {
        AddApplicableCharacter(playerType);
        _independentPenaltyBehvioursMap.Add(playerType, independentPenaltyBehviour);
//        _independentPenaltyBehviour = independentPenaltyBehviour;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayableCharacter playableCharacter_temp = other.GetComponent<PlayableCharacter>();
        if (playableCharacter_temp && ApplicableCharacters.Contains(playableCharacter_temp.PlayerType))
        {
            buttonInstance.SetActive(true);
            PatternButtonAnim.SetTrigger(MiniGame.ButtonAnimations[XboxButton.A]);
//            if (CharPos)
//            {
//                playableCharacter.disableMovement();
//                playableCharacter.transform.position = CharPos.position;
//                playableCharacter.PlayerController.isRight = CharFacingRight;
//            }
//            MiniGameScript.StartMiniGame(this, playableCharacter);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayableCharacter playableCharacter_temp = other.GetComponent<PlayableCharacter>();
        if (playableCharacter_temp && ApplicableCharacters.Contains(playableCharacter_temp.PlayerType))
        {
            if (XCI.GetButtonDown(XboxButton.A)) //todo - here problem
            {
                if (CharPos)
                {
                    playableCharacter_temp.disableMovement();
                    playableCharacter_temp.transform.position = CharPos.position;
                    playableCharacter_temp.PlayerController.isRight = CharFacingRight;
                }

                MiniGameScript.StartMiniGame(this, playableCharacter_temp);
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayableCharacter playableCharacter_temp = other.GetComponent<PlayableCharacter>();
        if (playableCharacter_temp && ApplicableCharacters.Contains(playableCharacter_temp.PlayerType))
        {
            buttonInstance.SetActive(false);
            MiniGameScript.EndMiniGame();
        }
    }


}
