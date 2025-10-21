using System;
using OpenCover.Framework.Model;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed = 2000f;

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackRadius;

    [Header("Weapon Info")]
    public Weapon weapon;

    #region 组件
    public Animator anim { get; private set; }
    public Animator weaponAnim;
    public Animator overlayAnim;
    // public CharacterController cc { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public CharacterStats stats { get; private set; }
    private SpriteRenderer sr;
    private SpriteRenderer weaponSr;
    public MyInputSystem inputActions { get; private set; }
    #endregion

    #region  状态
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerPickupState pickupState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    #endregion

    public GameObject target;

    [SerializeField]
    private Transform weaponParent;

    void Awake()
    {
        inputActions = new MyInputSystem();

        anim = GetComponentInChildren<Animator>();
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        anim.transform.rotation = Quaternion.Euler(-45f, 0f, 0f);

        // cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();

        

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        walkState = new PlayerWalkState(stateMachine, this, "Walk");
        pickupState = new PlayerPickupState(stateMachine, this, "Duck");
        attackState = new PlayerAttackState(stateMachine, this, "Attack");
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        stateMachine.Initialize(idleState);

        sr = anim.GetComponent<SpriteRenderer>();
        weaponSr = weaponParent.GetComponentInChildren<SpriteRenderer>();
    }



    void Update()
    {
        sr.sortingOrder = -((int)transform.position.y * 10 + (int)transform.position.x);
        weaponSr.sortingOrder = sr.sortingOrder - 1;
        stateMachine.currentState.Update();
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public Objects GetNearestObject()
    {
        float distance = Mathf.Infinity;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);
        Objects obj = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Objects>() != null && hit.GetComponent<Objects>().enabled)
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < distance)
                {
                    distance = d;
                    obj = hit.GetComponent<Objects>();
                }
            }
        }
        return obj;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 10f);
        Gizmos.DrawWireSphere(transform.position, 1.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }


    public void StopMove()
    {
        rb.velocity = Vector2.zero;
    }

    public void Flip(float x)
    {
        if (x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            anim.transform.rotation = Quaternion.Euler(-45f, 0f, 0f);
        }
        else if (x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            anim.transform.rotation = Quaternion.Euler(45f,180f,0f);
        }
    }

    public void SetWeapon(Weapon _weapon)
    {
        weapon = _weapon;
        if (weapon != null)
        {
            weaponParent.GetComponentInChildren<SpriteRenderer>().sprite = weapon.sprite;
        }
        else
        {
            weaponParent.GetComponentInChildren<SpriteRenderer>().sprite = null;
        }

    }
}
