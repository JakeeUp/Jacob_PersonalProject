using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIType
{
    Safe,
    Scared,
    Angry
}

public enum AIState
{
    Idle,
    Roaming,
    Attacking,
    Fleeing
}
public class Enemy : MonoBehaviour , IDamageable
{
    [Header("AI Settings")]
    public AIType aiType;
    private AIState aiState;
    public float detectDis, safeDis;

    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] drops;

    [Header("Roaming Settings")]
    public float minRoamDis;
    public float maxRoamDis;
    public float minRoamWaitTime;
    public float maxRoamWaitTime;


    [Header("Attack Settings")]
    public int dmg;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;
    private float playerDistance;
    public GameObject[] bloodTrack;

    [Header("Component")]
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        SetState(AIState.Roaming);
    }
    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
        animator.SetBool("Move", aiState != AIState.Idle);
        switch(aiState)
        {
            case AIState.Idle: PassiveUpdate();
                break;
            case AIState.Roaming: PassiveUpdate();
                break;
            case AIState.Attacking: AttackUpdate();
                break;
            case AIState.Fleeing: FleeUpdate();
                break;
        }
    }

    void PassiveUpdate()
    {
        if(aiState == AIState.Roaming && agent.remainingDistance < .1f)
        {
            SetState(AIState.Idle);
            Invoke("RoamToNewLoc", Random.Range(minRoamWaitTime,maxRoamWaitTime));
        }

        if(aiType == AIType.Angry && playerDistance < detectDis)
        {
            SetState(AIState.Attacking);
        }

        if(aiType == AIType.Scared && playerDistance < detectDis)
        {
            SetState(AIState.Fleeing);
            agent.SetDestination(GetFleeLoc());
        }
    }

    void AttackUpdate()
    {
        if(playerDistance > attackDistance)
        {
            agent.SetDestination(PlayerController.instance.transform.position);
        }
        else
        {
            agent.isStopped = true;
            if(Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                animator.SetTrigger("Attack");
            }
        }
    }

    public void DoDmg()
    {
        PlayerController.instance.GetComponent<IDamageable>().TakeDamage(dmg);
    }
    public void Chase()
    {
        agent.isStopped = false;

    }
    public void StopEnemy()
    {
        agent.isStopped = true;
    }

    void FleeUpdate()
    {
        if (playerDistance < safeDis && agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(GetFleeLoc());

        }
        else if(playerDistance > safeDis)
        {
            SetState(AIState.Roaming);
        }

    }

    void SetState(AIState newState)
    {
        aiState = newState;

        switch(aiState)
        {
            case AIState.Idle:
                {
                    agent.speed = walkSpeed;
                    agent.isStopped = true;
                    break;
                }
            case AIState.Roaming:
                {
                    agent.speed = walkSpeed;
                    agent.isStopped = false;
                    break;
                }
            case AIState.Attacking:
                {
                    agent.speed = runSpeed;
                    agent.isStopped = false;
                    break;
                }
            case AIState.Fleeing:
                {
                    agent.speed = runSpeed;
                    agent.isStopped = false;
                    break;
                }
              
        }
    }
    void RoamToNewLoc()
    {
        if (aiState != AIState.Idle)
            return;
        SetState(AIState.Roaming);
        agent.SetDestination(GetRoamingPos());
    }

    Vector3 GetRoamingPos()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + 
            (Random.onUnitSphere * Random.Range
            (minRoamDis, maxRoamDis)),
            out hit, 
            maxRoamDis,
            NavMesh.AllAreas);

        int i = 0;

        while(Vector3.Distance(transform.position, hit.position) < detectDis)
        {
            NavMesh.SamplePosition(transform.position +
            (Random.onUnitSphere * Random.Range
            (minRoamDis, maxRoamDis)),
            out hit,
            maxRoamDis,
            NavMesh.AllAreas);

            i++;
            if (i == 35) break;
        }
        return hit.position;
    }

    Vector3 GetFleeLoc()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position +
            (Random.onUnitSphere * safeDis), out hit,
            safeDis, NavMesh.AllAreas);

        int i = 0;

        while(GetDestinationAngle(hit.position) > 90 || playerDistance < safeDis)
        {
            NavMesh.SamplePosition(transform.position +
            (Random.onUnitSphere * safeDis), out hit,
            safeDis, NavMesh.AllAreas);

            i++;
            if(i == 35)
            {
                break;
            }
        }
        return hit.position;
    }

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - PlayerController.instance.transform.position, transform.position + targetPos);
    }

    public void TakeDamage(int DamageAmount)
    {
        health -= DamageAmount;
        animator.SetTrigger("Hit");
        GameObject obj = Instantiate(bloodTrack[Random.Range(0, bloodTrack.Length)], transform.position, Quaternion.identity);
        Destroy(obj, 3);

        if(health <= 0)
        {
            Die();
        }

        if(aiType == AIType.Safe && health > 0)
        {
            SetState(AIState.Fleeing);
        }
    }

    void Die()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        for(int x = 0; x < drops.Length; x++)
        {
            Instantiate(drops[x].dropPrefab, transform.position, Quaternion.identity);
        }
        animator.SetBool("Death", true);
        Destroy(gameObject,5);
    }
}
