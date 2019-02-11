using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XboxCtrlrInput;
using Random = UnityEngine.Random;

public class ActionableObject : MonoBehaviour
{

    enum State {IDLE, READY, MINIGAME};

    private State state = State.IDLE;
    
    private Animator _animator;
    
    [System.Serializable]
    struct BrokenMiniGame
    {
        public bool CanBeBroke;
        public MiniGame BrokenMiniGameScript;
        public int minBrokenTime;
        public int maxBrokenTime;
//        public PlayableCharacter dad;
//        public Sprite needIcon;
    }
    
    public bool isExplicit;
    public int stressImpact;
    public bool hasGlobalImpact;
    public Transform PatternButtonPos;
    [HideInInspector] public GameObject buttonInstance;
    [HideInInspector] public Animator PatternButtonAnim;
    public GameObject PatternButton;
    
    public Transform CharPos;
    public Transform CharExitMiniGamePos;
    public bool CharFacingRight = true;
    public List<GameManager.PlayerType> ApplicableCharacters;

    [SerializeField] private String fromIdleToReadyStateAnimation;
    
    private UnityAction playerMoveInFixPos;

//    public Sprite idleState;
//    public Sprite needsActionState;

    public MiniGame MiniGameScript;
    [HideInInspector] internal MiniGame currentMiniGameInPlay;

//    private PlayableCharacter playableCharacter;

    [SerializeField] private bool removeAllPlayer;
    
    private bool isBroken;
    
    public bool IsBroken {
        get
        {
            return isBroken;
        }
        set
        {
            isBroken = value;
            _animator.SetBool("broken", isBroken);
//            if (isBroken)
//            {
//                if (!_brokenMiniGame.dad.needsSprites.Contains(_brokenMiniGame.needIcon))
//                {
//                    _brokenMiniGame.dad.needsSprites.Add(_brokenMiniGame.needIcon);
//                }
//            }
//            else
//            {
//                _brokenMiniGame.dad.needsSprites.Remove(_brokenMiniGame.needIcon);
//            }

        }
    }


    private Dictionary<GameManager.PlayerType, PenaltyBehaviour> _PenaltyBehavioursMap =
        new Dictionary<GameManager.PlayerType, PenaltyBehaviour>();
//    private IndependentPenaltyBehviour _independentPenaltyBehviour;


    [SerializeField] private BrokenMiniGame _brokenMiniGame;

    [SerializeField] private bool isDogPoop = false;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMoveInFixPos = () =>
        {
            if (currentMiniGameInPlay) currentMiniGameInPlay.EndMiniGame();
        };
        if (!buttonInstance)
            buttonInstance = (GameObject)Instantiate(PatternButton, PatternButtonPos.position, Quaternion.identity);
        PatternButtonAnim = buttonInstance.GetComponent<Animator>();
        buttonInstance.SetActive(false);

        if (_brokenMiniGame.CanBeBroke)
        {
            StartCoroutine(nameof(broke));
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.IDLE:
                if (ApplicableCharacters.Count > 0)
                {
                    state = State.READY;
                    if (_animator)
                        _animator.SetTrigger(fromIdleToReadyStateAnimation);
                }
                break;
            case State.READY:
                break;
            case State.MINIGAME:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnMiniGameSuccess(GameManager.PlayerType playerType)
    {
        // Reset Actionable state
        print("OnMiniGameSuccess");
//        _independentPenaltyBehviour.Off();    
        
        
        if (IsBroken)
        {
            IsBroken = false;
        }
        else
        {
            ApplicableCharacters.Remove(playerType);
            
            if (removeAllPlayer)
            {
                foreach (GameManager.PlayerType key in _PenaltyBehavioursMap.Keys)
                {
                    _PenaltyBehavioursMap[key].Off();
                }
                _PenaltyBehavioursMap.Clear();
            }
            else if (_PenaltyBehavioursMap.ContainsKey(playerType))
            {
                _PenaltyBehavioursMap[playerType].Off();
                _PenaltyBehavioursMap.Remove(playerType);
            }
        }

        if (isDogPoop)
        {
            Destroy(gameObject);
        }
    }

    public void OnMiniGameEnd(PlayableCharacter player, bool miniGameSuccess)
    {
        if (miniGameSuccess)
        {
            OnMiniGameSuccess(player.PlayerType);
            player.ApplyStressImpact(stressImpact);
        }
        
        if(PatternButtonAnim)
            buttonInstance.SetActive(false);
        currentMiniGameInPlay = null;
        state = State.IDLE;
    }

    public void AddApplicableCharacter(GameManager.PlayerType playerType)
    {
        ApplicableCharacters.Add(playerType);
    }

    public void AddApplicableCharacter(GameManager.PlayerType playerType, PenaltyBehaviour penaltyBehaviour)
    {
        AddApplicableCharacter(playerType);
        _PenaltyBehavioursMap.Add(playerType, penaltyBehaviour);
//        _independentPenaltyBehviour = independentPenaltyBehviour;
    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        PlayableCharacter playableCharacter_temp = other.GetComponent<PlayableCharacter>();
//        if (playableCharacter_temp && ApplicableCharacters.Contains(playableCharacter_temp.PlayerType))
//        {
//            buttonInstance.SetActive(true);
//            PatternButtonAnim.SetTrigger(MiniGame.ButtonAnimations[XboxButton.A]);
////            if (CharPos)
////            {
////                playableCharacter.disableMovement();
////                playableCharacter.transform.position = CharPos.position;
////                playableCharacter.PlayerController.isRight = CharFacingRight;
////            }
////            MiniGameScript.StartMiniGame(this, playableCharacter);
//        }
//    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (MiniGameScript.inMiniGame || (_brokenMiniGame.CanBeBroke && _brokenMiniGame.BrokenMiniGameScript.inMiniGame))
            return;
        PlayableCharacter playableCharacter_temp = other.GetComponent<PlayableCharacter>();

        if (!playableCharacter_temp)
            return;
        if (!IsBroken)
        {
            if (state == State.READY && ApplicableCharacters.Contains(playableCharacter_temp.PlayerType))
            {
                currentMiniGameInPlay = MiniGameScript;
                EnterMiniGame(currentMiniGameInPlay, playableCharacter_temp);
            }
        }
        else
        {
            if (playableCharacter_temp.PlayerType == GameManager.PlayerType.Dad)
            {
                currentMiniGameInPlay = _brokenMiniGame.BrokenMiniGameScript;
                EnterMiniGame(currentMiniGameInPlay, playableCharacter_temp);
            }
        }
    }


    private void EnterMiniGame(MiniGame miniGameToEnter, PlayableCharacter playableCharacter)
    {
        if (!buttonInstance.activeSelf)
        {
            buttonInstance.SetActive(true);
            PatternButtonAnim.SetTrigger(MiniGame.ButtonAnimations[XboxButton.A]);
        }
        else if (XCI.GetButtonDown(XboxButton.A, playableCharacter.PlayerController.controller) && playableCharacter.isAlive)
        {
            PatternButtonAnim.SetTrigger("white");
            if (CharPos)
            {
//                if (!miniGameToEnter.desapireInEnter)
                playableCharacter.disableMovement(playerMoveInFixPos);
                playableCharacter.transform.position = CharPos.position;
                playableCharacter.PlayerController.isRight = CharFacingRight;
                stickTime = Time.fixedTime;
            }

            state = State.MINIGAME;
            miniGameToEnter.StartMiniGame(this, playableCharacter);
        }
       
    }



    private float stickTime;

    void OnTriggerExit2D(Collider2D other)
    {
        if (Time.fixedTime - stickTime < 0.5f)
            return;
        print("trigger exit");
        PlayableCharacter playableCharacter_temp = other.GetComponent<PlayableCharacter>();
        if (playableCharacter_temp && currentMiniGameInPlay && !currentMiniGameInPlay.player) //ApplicableCharacters.Contains(playableCharacter_temp.PlayerType) && 
        {
            currentMiniGameInPlay.EndMiniGame(triggerExit: true);
            OnMiniGameEnd(null, false);
        }
    }

    #region Broken

    IEnumerator broke()
    {
        while (true)
        {
            yield return new WaitUntil(() => !IsBroken); 
            yield return new WaitForSeconds(Random.Range(_brokenMiniGame.minBrokenTime, _brokenMiniGame.maxBrokenTime));
            yield return new WaitUntil(() => !MiniGameScript.inMiniGame);
            IsBroken = true;
        }
    }

    #endregion


}
