using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionableObject : MonoBehaviour
{

    public bool isExplicit;
    public int stressImpact;
    public bool hasGlobalImpact;

    public Sprite idleState;
    public Sprite needsActionState;
    public GameObject ActionIcon;

    public MiniGame MiniGameScript;

    private PlayableCharacter playableCharacter;

    private GameObject ActionIconInstance;
    private bool isBroken;
    private bool needsAction;


    // Start is called before the first frame update
    void Start()
    {
        needsAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        NeedUpdate();

    }

    // Check whether needsAction should be turned on and if yes, turns it on.
    void NeedUpdate()
    {

    }

    public void OnMiniGameSuccess()
    {
        // Reset Actionable state
        needsAction = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print(gameObject.name + "triggered with" + other.gameObject.name);
        playableCharacter = other.GetComponent<PlayableCharacter>();
        if (playableCharacter && needsAction)
        {
            MiniGameScript.StartMiniGame(this, playableCharacter);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (ActionIconInstance)
        {
            Destroy(ActionIconInstance);
        }
    }


}
