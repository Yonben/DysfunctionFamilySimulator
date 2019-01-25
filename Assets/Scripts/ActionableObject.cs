using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionableObject : MonoBehaviour
{

    public bool isExplicit;
    public int stressImpact;
    public bool hasGlobalImpact;
    public Transform PatternButtonPos;

    public Sprite idleState;
    public Sprite needsActionState;

    public MiniGame MiniGameScript;

    private PlayableCharacter playableCharacter;

    private bool isBroken;
    private bool needsAction = true;

    // Start is called before the first frame update
    void Start()
    {
        PatternButtonPos = this.gameObject.transform.GetChild(0);
    }

    // Check whether needsAction should be turned on and if yes, turns it on.
    void NeedUpdate()
    {
        needsAction = true;
        print("Need !!");
    }

    public void OnMiniGameSuccess()
    {
        // Reset Actionable state
        needsAction = false;
        Invoke("NeedUpdate", 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playableCharacter = other.GetComponent<PlayableCharacter>();
        if (playableCharacter && needsAction)
        {
            // var currentPosition = gameObject.transform.position;
            // var iconPosition = new Vector3(currentPosition.x, currentPosition.y + 0.15f, currentPosition.z);
            // ActionIconInstance = Instantiate(ActionIcon, iconPosition, Quaternion.identity);

            MiniGameScript.StartMiniGame(this, playableCharacter);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        MiniGameScript.EndMiniGame();
    }


}
