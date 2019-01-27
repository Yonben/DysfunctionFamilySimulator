using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class ActionableObject : MonoBehaviour
{

    private Animator _animator;
    
    [System.Serializable]
    struct BrokenMiniGame
    {
        public bool CanBeBroke;
        public MiniGame BrokenMiniGameScript;
        public int minBrokenTime;
        public int maxBrokenTime;
        public PlayableCharacter dad;
        public Sprite needIcon;
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

    public Sprite idleState;
    public Sprite needsActionState;

    public MiniGame MiniGameScript;

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
            if (isBroken)
            {
                if (!_brokenMiniGame.dad.needsSprites.Contains(_brokenMiniGame.needIcon))
                {
                    _brokenMiniGame.dad.needsSprites.Add(_brokenMiniGame.needIcon);
                }
            }
            else
            {
                _brokenMiniGame.dad.needsSprites.Remove(_brokenMiniGame.needIcon);
            }

        }
    }


    private Dictionary<GameManager.PlayerType, IndependentPenaltyBehviour> _independentPenaltyBehvioursMap =
        new Dictionary<GameManager.PlayerType, IndependentPenaltyBehviour>();
//    private IndependentPenaltyBehviour _independentPenaltyBehviour;


    [SerializeField] private BrokenMiniGame _brokenMiniGame;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!buttonInstance)
            buttonInstance = (GameObject)Instantiate(PatternButton, PatternButtonPos.position, Quaternion.identity);
        PatternButtonAnim = buttonInstance.GetComponent<Animator>();
        buttonInstance.SetActive(false);

        if (_brokenMiniGame.CanBeBroke)
        {
            StartCoroutine(nameof(broke));
        }
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
        else if (_independentPenaltyBehvioursMap.ContainsKey(playerType))
        {
            _independentPenaltyBehvioursMap[playerType].Off();
            _independentPenaltyBehvioursMap.Remove(playerType);
        }


        if (IsBroken)
        {
            IsBroken = false;
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
        if (MiniGameScript.inMiniGame)
            return;
        PlayableCharacter playableCharacter_temp = other.GetComponent<PlayableCharacter>();

        if (!playableCharacter_temp)
            return;
        if (!IsBroken)
        {
            if (ApplicableCharacters.Contains(playableCharacter_temp.PlayerType))
            {
                EnterMiniGame(MiniGameScript, playableCharacter_temp);
            }
        }
        else
        {
            if (playableCharacter_temp.PlayerType == GameManager.PlayerType.Dad)
            {
                EnterMiniGame(_brokenMiniGame.BrokenMiniGameScript, playableCharacter_temp);
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
        else if (XCI.GetButtonDown(XboxButton.A, playableCharacter.PlayerController.controller))
        {
            PatternButtonAnim.SetTrigger("white");
            if (CharPos)
            {
                if (!miniGameToEnter.desapireInEnter)
                    playableCharacter.disableMovement();
                playableCharacter.transform.position = CharPos.position;
                playableCharacter.PlayerController.isRight = CharFacingRight;
                stickTime = Time.fixedTime;
            }
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
        if (playableCharacter_temp && ApplicableCharacters.Contains(playableCharacter_temp.PlayerType))
        {
            buttonInstance.SetActive(false);
            MiniGameScript.EndMiniGame(triggerExit: true);
        }
    }

    #region Broken

    IEnumerator broke()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_brokenMiniGame.minBrokenTime, _brokenMiniGame.maxBrokenTime));
            yield return new WaitUntil(() => !MiniGameScript.inMiniGame);
            IsBroken = true;
        }
    }

    #endregion


}
