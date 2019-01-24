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
    public Sprite ActionIcon;
    private bool isBroken;
    private bool needsAction;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print(gameObject.name + "triggered with" + other.gameObject.name);
        PlayableCharacter playableCharacter = other.GetComponent<PlayableCharacter>();
        if (playableCharacter)
        {
            playableCharacter.ApplyStressImpact(0);
        }
    }


}
