using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState
{
    CHASE,
    ATTACK
}

public class CharacterController : CharacterBase
{
    private CharAnimations characterAnim;
    private Animator characterAnimator;
    private NavMeshAgent navAgent;

    public GameObject[] attackPoints;

    private Transform target;
    public float moveSpeed = 3f;
    public float attackDistance = 1.5f;
    public float chaseAfterAttackDistance = 1f;
    public float rotationSpeed = 90f;
    public float attackDelay = 3f;
    public float attackAnimationSpeed = 1f;
    public bool isAllies;

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

    private CharacterState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        characterAnim = GetComponent<CharAnimations>();
        characterAnimator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        

        enemyState = CharacterState.CHASE;
        waitTimer = attackDelay;
        characterAnimator.speed = attackAnimationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        target = FindClosesTarget();
        if(target == null)
        {
            return;
        }
        switch (enemyState)
        {
            case CharacterState.CHASE:
                ChasePlayer();
                break;
            case CharacterState.ATTACK:
                AttackPlayer();
                break;
        }
        if (!characterAnim.IsDoingAttackAndStuff())
        {
            RotateTowardPlayer();
        }

    }
    private Transform FindClosesTarget()
    {
        List<GameObject> candidates = new List<GameObject>();
        if (!isAllies)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.PLAYER_TAG);
            GameObject[] allies = GameObject.FindGameObjectsWithTag(Tags.ALLIES_TAG);
            candidates.AddRange(players);
            candidates.AddRange(allies);
        }
        else
        {
            candidates.AddRange(GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG));
        }
        if(candidates.Count == 0)
        {
            return null;
        }

        int nearestIndex = -1;
        float nearestValueDistance = Vector3.Distance(candidates[0].transform.position, transform.position);
        for (int i = 0; i < candidates.Count; i++)
        {
            HealthScript tempHC = candidates[i].GetComponent<HealthScript>();
            if (tempHC != null && !tempHC.IsCharDead())
            {
                if (nearestValueDistance >= Vector3.Distance(candidates[i].transform.position, transform.position))
                {
                    nearestIndex = i;
                    nearestValueDistance = Vector3.Distance(candidates[i].transform.position, transform.position);
                }
            }
        }
        if(nearestIndex != -1)
        {
            return candidates[nearestIndex].transform;
        }
        return null;

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
        if(Vector3.Distance(transform.position, target.position) > attackDistance + chaseAfterAttackDistance && !isAttacking)
        {
            navAgent.isStopped = false;
            enemyState = CharacterState.CHASE;
        }
    }

    private IEnumerator MoveInAndAttack(AttackPattern pattern)
    {
        isAttacking = true;

        Vector3 startPos = transform.position;

        Vector3 directionToTarget = (target.position - startPos).normalized;
        Vector3 stopPoint = target.position - directionToTarget * pattern.moveDistance;

        float startAttackTime = Time.time + 0.4f;///// sometime AI stuck when they can't move to stopPoint @@
        while (Vector3.Distance(transform.position, stopPoint) > 0.1f || startAttackTime>=Time.time)
        {
            transform.position = Vector3.MoveTowards(transform.position, stopPoint, Time.deltaTime * moveSpeed);
            yield return null;
        }

        characterAnimator.SetTrigger(pattern.animationTrigger);
        attackPointsToActive = pattern.attackPoints;
        characterAnim.ActionAnimationStart();
        yield return new WaitUntil(() => !characterAnim.IsDoingAttackAndStuff());

        float startBackTime = Time.time + 0.4f;///// sometime AI stuck when they can't move to stopPoint @@
        while (Vector3.Distance(transform.position, startPos) > .1f || startBackTime>= Time.time)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * moveSpeed);
            yield return null;
        }
        transform.position = startPos;

        waitTimer = 0f;

        isAttacking = false;
    }

    public void RotateTowardPlayer()
    {
        GameObject enemy = target.gameObject;
        
        Vector3 direction = enemy.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void ChasePlayer()
    {
        navAgent.SetDestination(target.position);
        navAgent.speed = moveSpeed;

        if(navAgent.velocity.sqrMagnitude == 0)
        {
            AnimateMovement(Vector2.zero, Vector3.zero);
        }
        else
        {
            AnimateMovement(new(0, 1),navAgent.velocity);
        }
        if(Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            enemyState = CharacterState.ATTACK;
        }
    }

    private void AnimateMovement(Vector2 movement, Vector3 move)
    {
        characterAnim.WalkingDirection(AnimatorParams.WALKLR, movement.x);
        characterAnim.WalkingDirection(AnimatorParams.WALKFB, movement.y);
        characterAnim.Walk(move != Vector3.zero);
    }

}
