using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[RequireComponent(typeof(PlayerController))]
public class PlayableCharacter : MonoBehaviour
{

    [SerializeField] [Range(0, 100)] private int stress;
    [Range(0, 100)] private int stressMinThresholds;
    [SerializeField] private int maxStress = 100; 
    private PlayerController PlayerController;
    
    private Rigidbody2D m_Rigidbody2D;
    private Animator m_animator;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        m_animator.SetFloat("velocity", m_Rigidbody2D.velocity.magnitude);
    }

    public void ApplyStressImpact(int stressImpact)
    {
        stress += stressImpact;
        stress = Mathf.Max(stress, stressMinThresholds);
        print("ApplyStressImpact");

        if (stress >= maxStress)
        {
            Die();
        }
    }

    private void Die()
    {
        //lock stress??
        
        //remove the player visually with animation
        //todo
        
        // remove player control.
        enabledControl(false);
        
        //die on GameManager
        GameManager.instance.PlayerDie(this);
    }

    public void enabledMovement(bool enable=true)
    {
        PlayerController.canMove = enable;
    }
    
    private void enabledControl(bool enable=true)
    {
        enabledMovement(enable);
        //todo - also enable buttons.
    }
}
