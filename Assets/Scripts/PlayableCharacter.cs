using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class PlayableCharacter : MonoBehaviour
{

    [SerializeField] [Range(0, 100)] private int stress;
    [Range(0, 100)] private int stressMinThresholds;
    [SerializeField] private int maxStress = 100;
    [HideInInspector] public PlayerController PlayerController;

    private Rigidbody2D m_Rigidbody2D;
    public Animator m_animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    [SerializeField] private Slider stressSlider;

    public GameManager.PlayerType PlayerType;

    [SerializeField] private SpriteRenderer needsSpriteRenderer;
    [HideInInspector] public List<Sprite> needsSprites;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stressSlider.value = (float)(stress) / (float)maxStress;
    }

    private void Start()
    {
        StartCoroutine(nameof(needsHandler));
    }

    private void Update()
    {
        m_animator.SetFloat("velocity", m_Rigidbody2D.velocity.magnitude);
    }

    public void ApplyStressImpact(int stressImpact)
    {
        stress += stressImpact;
        stress = Mathf.Max(stress, stressMinThresholds);

        stressSlider.value = (float)(stress) / (float)maxStress;

        if (stress >= maxStress)
        {
            Die();
        }
    }

    private void Die()
    {
        print("player die, " + gameObject.name);
        //lock stress??

        //remove the player visually with animation
        //todo

        // remove player control.
        enabledControl(false);

        //die on GameManager
        GameManager.instance.PlayerDie(this);
    }

    public void enabledMovement(bool enable = true)
    {
        m_Rigidbody2D.constraints = enable ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
        PlayerController.canMove = enable;
    }

    private void enabledControl(bool enable = true)
    {
        enabledMovement(enable);
        //todo - also enable buttons.
    }

    public void disableMovement()
    {
        enabledMovement(false);
        StartCoroutine(nameof(enableMovement));
        
    }
    
    IEnumerator enableMovement() 
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => PlayerController.isTryMoove());
        enabledMovement(true);
    }

    IEnumerator needsHandler()
    {
        Sprite lastNeed = null;
        while (true)
        {
            if (needsSprites.Count == 0)
            {
                needsSpriteRenderer.sprite = null;
                yield return new WaitWhile(() => needsSprites.Count == 0);
            }
//            print("we have need");
            
            if (lastNeed)
                lastNeed = needsSprites[(needsSprites.IndexOf(lastNeed) + 1) % needsSprites.Count]; //index of return -1 when not found.
            else
            {
                lastNeed = needsSprites[0];
            }

            needsSpriteRenderer.sprite = lastNeed;
            yield return new WaitForSeconds(1f);
        }
    }
}
