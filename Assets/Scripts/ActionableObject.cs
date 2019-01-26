﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionableObject : MonoBehaviour
{

    public bool isExplicit;
    public int stressImpact;
    public bool hasGlobalImpact;
    public Transform PatternButtonPos;
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
        playableCharacter = other.GetComponent<PlayableCharacter>();
        if (playableCharacter && ApplicableCharacters.Contains(playableCharacter.PlayerType))
        {
            if (CharPos)
            {
                playableCharacter.disableMovement();
                playableCharacter.transform.position = CharPos.position;
                playableCharacter.PlayerController.isRight = CharFacingRight;
            }
            MiniGameScript.StartMiniGame(this, playableCharacter);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        MiniGameScript.EndMiniGame();
    }


}
