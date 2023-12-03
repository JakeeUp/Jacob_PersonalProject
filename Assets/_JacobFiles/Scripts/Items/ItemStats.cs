using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    [Header("Iten Stats")]
    [SerializeField] float _attackRate;
    [SerializeField] bool _attacking;
    [SerializeField] float _attackDistance;
    [SerializeField] bool _DoesGatherResources;
    [SerializeField] bool _DoesDealDmg;
    [SerializeField] int _dmg;

    public float attackRate
    {
        get { return _attackRate; }
        set { _attackRate = value; }
    }

    public bool attacking
    {
        get { return _attacking; }
        set { _attacking = value; }
    }

    public float attackDistance
    {
        get { return _attackDistance; }
        set { _attackDistance = value; }
    }

    public bool DoesGatherResources
    {
        get { return _DoesGatherResources; }
        set { _DoesGatherResources = value; }
    }

    public bool DoesDealDmg
    {
        get { return _DoesDealDmg; }
        set { _DoesDealDmg = value; }
    }

    public int dmg
    {
        get { return _dmg; }
        set { _dmg = value; }
    }
}
