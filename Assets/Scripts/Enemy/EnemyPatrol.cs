using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    Animator animator;
    Rigidbody2D RB;

    [SerializeField] Transform patrolRight;
    [SerializeField] Transform patrolLeft;

    private GameObject playerObject;

    private int MoveDirection = 1;

    private bool _isAttackRange;
    private bool _isPlayerInSight;
    private bool _isJustExitedAttack;
    private bool _isAttackMotion;
    private bool turnRight;
    private bool turnLeft;

    public bool DoDamage;
    public bool IsStunned;

    public float VisionRange;
    public float AttackRange;

    public bool IsAttacking { get; private set; }


    [SerializeField] private LayerMask _playerLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.localScale = new Vector3(-0.75f, 0.75f, 0.75f);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RB.linearVelocityX < -0.5f || RB.linearVelocityX > 0.5f)
            animator.Play("Walk");

        if (Physics2D.OverlapCircle(transform.position, VisionRange, _playerLayer))
            _isPlayerInSight = true;
        else
            _isPlayerInSight = false;

        if (Physics2D.OverlapCircle(transform.position, AttackRange, _playerLayer))
        {
            _isAttackRange = true;
        }
        else
            _isAttackRange = false;

        if (_isPlayerInSight)
        {
            if (_isAttackRange)
            {
                if (!IsAttacking && !_isAttackMotion && !IsStunned)
                {
                    //Attack Method
                }
            }
        }
        if ((_isPlayerInSight && !_isAttackRange && !_isJustExitedAttack) || (!_isPlayerInSight && !_isAttackRange && !_isJustExitedAttack) && !IsStunned && !IsAttacking)
        {
            Patrol();
        }
    }

    #region PATROL METHODS
    private void Patrol()
    {
        float distancePatrolLeft = Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(patrolLeft.position.x, 0, 0));
        float distancePatrolRight = Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(patrolRight.position.x, 0, 0));
        float distancePlayer = Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(playerObject.transform.position.x, 0, 0));

        if (!_isPlayerInSight)
        {
            if ((distancePatrolLeft < 1f && distancePatrolLeft > 1f) || (patrolLeft.transform.position.x > transform.position.x))
            {
                turnRight = true;
                turnLeft = false;
            }
            if ((distancePatrolRight < 1f && distancePatrolRight > 1f) || (patrolRight.transform.position.x < transform.position.x))
            {
                turnLeft = true;
                turnRight = false;
            }
        }
        else
        {
            if (playerObject.transform.position.x > transform.position.x)
            {
                turnRight = true;
                turnLeft = false;
            }
            else if (playerObject.transform.position.x < transform.position.x)
            {
                turnLeft = true;
                turnRight = false;
            }
        }

        #region Calculate Face Direction
        if (turnRight)
        {
            gameObject.transform.localScale = new Vector3(-0.75f, 0.75f, 0.75f);
            MoveDirection = 1;
        }
        else if (turnLeft)
        {
            gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            MoveDirection = -1;
        }
        #endregion

        RB.linearVelocityX = 2.5f * MoveDirection;
    }
  
    #endregion

    #region ATTACK METHODS

    #endregion

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, VisionRange);
    }
    #endregion
}