using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class PlayableCharacter : MonoBehaviour
{

    [SerializeField] [Range(0, 100)] protected int stress;
    [Range(0, 100)] private int stressMinThresholds;
    [SerializeField] protected int maxStress = 100;
    [HideInInspector] public PlayerController PlayerController;

    private Rigidbody2D m_Rigidbody2D;
    public Animator m_animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    [SerializeField] protected Slider stressSlider;

    public GameManager.PlayerType PlayerType;

    [SerializeField] private SpriteRenderer needsSpriteRenderer;
    [HideInInspector] public List<Sprite> needsSprites;

    [HideInInspector] public bool isAlive = true;

    protected virtual void Awake()
    {
        PlayerController = GetComponent<PlayerController>();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stressSlider.maxValue = maxStress;
        stressSlider.value = stress;
    }

    protected virtual void Start()
    {
        StartCoroutine(nameof(needsHandler));
    }

    protected virtual void Update()
    {
        m_animator.SetFloat("velocity", m_Rigidbody2D.velocity.magnitude);
    }

    public void ApplyStressImpact(int stressImpact)
    {
        stress += stressImpact;
        stress = Mathf.Max(stress, stressMinThresholds);

        stressSlider.value =stress;

        if (stress >= maxStress)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (!isAlive) //you can die only once
            return;
        
        
        print("player die, " + gameObject.name);
        //lock stress??

        //remove the player visually with animation
        //todo

        // remove player control.
        enabledControl(false);

        
        isAlive = false;
        
        //die on GameManager
        GameManager.instance.PlayerDie(this);
    }

    public void enabledMovement(bool enable = true)
    {
        if (isAlive)
        {
            m_Rigidbody2D.constraints =
                enable ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
            PlayerController.canMove = enable;
        }
    }

    private void enabledControl(bool enable = true)
    {
        enabledMovement(enable);
        //todo - also enable buttons.
        PlayerController.enabled = false;
    }

    public void disableMovement(UnityAction afterMovement = null)
    {
        enabledMovement(false);
        StartCoroutine(nameof(enableMovement), afterMovement);
        
    }
    
    IEnumerator enableMovement(UnityAction afterMovement = null) 
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => PlayerController.isTryMoove());
        enabledMovement(true);
        afterMovement.Invoke();
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
//            yield return new WaitForSeconds(1f);
//            YieldInstruction aa = new WaitForSeconds(1f);
            yield return new WaitForSecondsOrListChange<Sprite>(1, needsSprites);
        }
    }
    
    public class WaitForSecondsOrListChange<T> : CustomYieldInstruction
    {
        internal float m_Seconds;
        internal List<T> list;
        internal float listStartCount;
        internal float endTime;

        public override bool keepWaiting
        {
            get
            {
                return !(list.Count != listStartCount || Time.realtimeSinceStartup >= endTime);
            }
        }

        public WaitForSecondsOrListChange(float seconds, List<T> list)
        {
            this.m_Seconds = seconds;
            this.list = list;

            this.listStartCount = list.Count;
            this.endTime = Time.realtimeSinceStartup + seconds;
        }
    }
}
