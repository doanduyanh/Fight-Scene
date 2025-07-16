using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    CHASE,
    ATTACK
}

public class EnemyController : CharacterBase
{
    private CharAnimations enemyAnim;
    private Animator enemyAnimator;
    private NavMeshAgent navAgent;

    public GameObject[] attackPoints;

    private Transform playerTarget;
    public float moveSpeed = 3f;
    public float attackDistance = 1.5f;
    public float chaseAfterAttackDistance = 1f;
    public float rotationSpeed = 90f;
    public float attackDelay = 3f;
    public float attackAnimationSpeed = 1f;

    public void updateValue(CharStructure charStructure)
    {
        health = charStructure.health;
        damage = charStructure.damage;
        attackDelay = charStructure.attackDelay;
        attackAnimationSpeed = charStructure.attackSpeed;
        moveSpeed = charStructure.movementSpeed;

        
    }

    private float waitTimer;
    private bool isAttacking = false;

    private EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnim = GetComponent<CharAnimations>();
        enemyAnimator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        

        enemyState = EnemyState.CHASE;
        waitTimer = attackDelay;
        enemyAnimator.speed = attackAnimationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTarget == null)
        {
            playerTarget = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).GetComponent<Transform>();
        }
        switch (enemyState)
        {
            case EnemyState.CHASE:
                ChasePlayer();
                break;
            case EnemyState.ATTACK:
                AttackPlayer();
                break;
        }
        if (!enemyAnim.IsDoingAttackAndStuff())
        {
            RotateTowardPlayer();
        }

    }

    void AttackPlayer()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        AnimateMovement(Vector2.zero, Vector3.zero);

        waitTimer += Time.deltaTime;
        if(waitTimer > attackDelay && !isAttacking)
        {
            int randAttack = Random.Range(0, attackPatterns.Count);
            StartCoroutine(MoveInAndAttack(attackPatterns[randAttack]));
        }
        if(Vector3.Distance(transform.position, playerTarget.position) > attackDistance + chaseAfterAttackDistance && !isAttacking)
        {
            navAgent.isStopped = false;
            enemyState = EnemyState.CHASE;
        }
    }

    private IEnumerator MoveInAndAttack(AttackPattern pattern)
    {
        isAttacking = true;

        Vector3 startPos = transform.position;

        Vector3 directionToTarget = (playerTarget.position - startPos).normalized;
        Vector3 stopPoint = playerTarget.position - directionToTarget * pattern.moveDistance;

        while (Vector3.Distance(transform.position, stopPoint) > 0.1f)
        {
            float step = Time.deltaTime * moveSpeed; // adjust speed as needed
            transform.position = Vector3.MoveTowards(transform.position, stopPoint, step);
            yield return null;
        }

        enemyAnimator.SetTrigger(pattern.animationTrigger);
        attackPointsToActive = pattern.attackPoints;
        enemyAnim.ActionAnimationStart();
        yield return new WaitUntil(() => !enemyAnim.IsDoingAttackAndStuff());

        float remainingDistance = Vector3.Distance(transform.position, startPos);
        while (remainingDistance > .1f)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * moveSpeed);
            remainingDistance = Vector3.Distance(transform.position, startPos);
            yield return null;
        }
        transform.position = startPos;

        waitTimer = 0f;

        isAttacking = false;
    }

    public void RotateTowardPlayer()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG);
        
        Vector3 direction = enemy.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void ChasePlayer()
    {
        navAgent.SetDestination(playerTarget.position);
        navAgent.speed = moveSpeed;

        if(navAgent.velocity.sqrMagnitude == 0)
        {
            AnimateMovement(Vector2.zero, Vector3.zero);
        }
        else
        {
            AnimateMovement(new(0, 1),navAgent.velocity);
        }
        if(Vector3.Distance(transform.position, playerTarget.position) <= attackDistance)
        {
            enemyState = EnemyState.ATTACK;
        }
    }

    private void AnimateMovement(Vector2 movement, Vector3 move)
    {
        enemyAnim.WalkingDirection(AnimatorParams.WALKLR, movement.x);
        enemyAnim.WalkingDirection(AnimatorParams.WALKFB, movement.y);
        enemyAnim.Walk(move != Vector3.zero);
    }

}
