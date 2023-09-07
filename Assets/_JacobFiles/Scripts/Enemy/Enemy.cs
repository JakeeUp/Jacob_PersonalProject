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
public class Enemy : MonoBehaviour
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

    [Header("Component")]
    private NavMeshAgent agent;
    private Animator animator;

}
