using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{

    [SerializeField][Range(0,100)] private int stress;
    [Range(0,100)] private int stressMinThresholds;

    public void ApplyStressImpact(int stressImpact)
    {
        stress += stressImpact; 
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
