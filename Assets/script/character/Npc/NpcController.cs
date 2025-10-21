using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NpcController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    public int gridGraphIndex;
    public float idelWaitTimeout;

    public CheckPoint[] checkPoints { get; private set; }


    #region 组件
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Seeker seeker;
    public AIPath path;
    public AIDestinationSetter destinationSetter { get; private set; }
    private TextMeshProUGUI textMeshProUGUI;

    public Animator anim { get; private set; }
    #endregion

    public CharacterStateMachine stateMachine { get; private set; }

    #region 状态 
    public CharacterIdleState idleState { get; private set; }
    public CharacterWalkState walkState { get; private set; }
    public CharacterPlantState plantState { get; private set; }
    public CharacterAttackState attackState { get; private set; }
    #endregion

    public int index { get; private set; }
    private int preIndex;
    public string[] says;
    public List<Vector3Int> tiles;

    public Tilemap tilemap { get; private set; }

    [Header("gameobject")]
    public List<GameObject> gameObjects;
    public GameObject attackTarget;
    public Vector3 prePosition;
    public Transform gameObjectParent;

    void Awake()
    {
        tiles = new();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        seeker = GetComponent<Seeker>();
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        anim = GetComponentInChildren<Animator>();

        stateMachine = new CharacterStateMachine();

        idleState = new CharacterIdleState(stateMachine, this, "Idle");
        walkState = new CharacterWalkState(stateMachine, this, "Walk");
        plantState = new CharacterPlantState(stateMachine, this, "Idle");
        attackState = new CharacterAttackState(stateMachine, this, "Idle");
    }

    void Start()
    {
        stateMachine.Initialize(idleState);

        textMeshProUGUI.text = "";

        path = GetComponent<AIPath>();
        path.maxSpeed = moveSpeed;
        path.enableRotation = false;

        rb.isKinematic = false;
        // 平滑移动
        // rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        // 如果需要碰撞检测但仍不想被推动
        // rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        checkPoints = GetComponentsInChildren<CheckPoint>();
    }

    void Update()
    {
        sr.sortingOrder = -((int)transform.position.y * 10 + (int)transform.position.x);

        stateMachine.currentState.Update();
    }

    public void AddIndex()
    {
        index += 1;
    }

    public void AddIndex(int _length)
    {
        index++;
        if (index >= _length)
        {
            index = 0;
        }
    }

    public void StartWork(List<Vector3Int> _tiles, Tilemap _tileMap)
    {
        tiles = _tiles;
        tilemap = _tileMap;
    }

    public void StopDestinationSetter()
    {
        preIndex = index;
        index = -1;
        if (says == null || says.Length == 0) return;
        textMeshProUGUI.text = says[UnityEngine.Random.Range(0, says.Length)];
    }

    public void ReleaseDestinationSetter()
    {
        textMeshProUGUI.text = "";
        index = preIndex;
        stateMachine.ChangeState(idleState);
    }

    public void PlantGameObject()
    {
        if (gameObjects != null && gameObjects.Count > 0)
        {
            GameObject newObject = Instantiate(gameObjects[0], transform.position, Quaternion.identity,gameObjectParent);
            if (gameObjectParent.TryGetComponent<FacingCamara>(out FacingCamara facingCamara)){
                facingCamara.AddTransform(newObject.transform);
            }
            SpriteRenderer sr = newObject.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = -((int)transform.position.y * 10 + (int)transform.position.x);
            }

            if (newObject.TryGetComponent<CropBehaviour>(out CropBehaviour crop))
            {
                crop.SetOwned(gameObject);
            }
        }
    }

    public void SetAttackTarget(GameObject _attackTarget)
    {
        this.attackTarget = _attackTarget;
        if(attackTarget != null)
            prePosition = transform.position;
    }
}
