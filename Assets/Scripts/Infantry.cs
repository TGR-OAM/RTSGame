using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Infantry : Unit
{
    [Header("Navigation")]
    [SerializeField]
    private NavMeshAgent agent;


    private GameObject target;
    [Header("Behaviour properties")]
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private float attackDistance = 7f;
    [SerializeField]
    private float visionDistance = 10f;
    [SerializeField]
    private float damagePerSecond = 5f;
    private FractionMember fractionMember;

    private void Start()
    {
        fractionMember = GetComponent<FractionMember>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        SetState();
        Act();
    }

    private void UpdateTarget()
    {
        float smallestDst = 1000000;
        GameObject o = null;
        foreach (GameObject g in fractionMember.lister.units)
        {
            float d = Vector3.Distance(transform.position, g.transform.position);
            if (g.GetComponent<FractionMember>().fraction != fractionMember.fraction && d < smallestDst && d < visionDistance)
            {
                o = g;
            }
        }
        target = o;
    }

    private void Act()
    {
        switch (state)
        {
            case UnitState.Moving:
                agent.isStopped = false;
                break;
            case UnitState.Idle:
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
                break;
            case UnitState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
                break;
            case UnitState.Attacking:
                agent.isStopped = true;
                Attack();
                break;
            case UnitState.AttackMoving:
                agent.isStopped = false;
                agent.SetDestination(destination);
                UpdateTarget();
                break;
        }
    }

    private void Attack()
    {
        transform.LookAt(target.transform.position);
        target.GetComponent<DamageSystem>().TakeDamage(damagePerSecond * Time.deltaTime);
    }

    private void SetState()
    {
        if (gameOrder == null || gameOrder.isPefrormed)
        {
            if (target == null)
            {
                state = UnitState.Idle;
            } else 
            {
                state = UnitState.Attacking;
            }
        }
        if (gameOrder is MoveTask)
        {
            if (Vector3.Distance(transform.position, destination) >= reachDistance)
            {
                state = UnitState.Moving;
            } else
            {
                CompleteTask();
            }
        }
        if (gameOrder is AttackTask)
        {
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
                {
                    state = UnitState.Chasing;
                }
                else
                {
                    state = UnitState.Attacking;
                }
            } else
            {
                CompleteTask();
            }
        }
        if (gameOrder is MoveAttackTask)
        {
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
                {
                    state = UnitState.Chasing;
                }
                else
                {
                    state = UnitState.Attacking;
                }
            } else
            {
                if (Vector3.Distance(transform.position, destination) > reachDistance)
                {
                    state = UnitState.AttackMoving;
                }
                else
                {
                    CompleteTask();
                }
            }
        }
    }

    public override void GiveOrder(GameOrder order)
    {
        base.GiveOrder(order);
        if (order is MoveTask)
        {
            MoveTask t = (MoveTask)order;
            destination = t.destination;
            agent.SetDestination(destination);
        }
        if (order is AttackTask)
        {
            AttackTask t = (AttackTask)order;
            target = t.target;
        }
        if (order is MoveAttackTask)
        {
            MoveAttackTask t = (MoveAttackTask)order;
            destination = t.destination;
            agent.SetDestination(destination);
        }
    }

    public override void CompleteTask()
    {
        target = null;
        destination = transform.position;
        base.CompleteTask();
    }
}
