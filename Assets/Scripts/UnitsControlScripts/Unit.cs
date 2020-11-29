using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : OrderableObject
{
    //[Header("Navigation")]
    [SerializeField]
    public NavMeshAgent agent { get; private set; }


    private GameObject target;
    [Header("Behaviour properties")]
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private float reachDistance;
    [SerializeField]
    private float attackDistance = 7f;
    [SerializeField]
    private float visionDistance = 10f;
    [SerializeField]
    private float damagePerSecond = 5f;
    private FractionMember fractionMember;

    private void Start()
    {
        orderTypes = new List<Type> {typeof(MoveAttackTask), typeof (MoveTask), typeof(AttackTask)};
        fractionMember = GetComponent<FractionMember>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateOrder();
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
    private void Attack()
    {
        transform.LookAt(target.transform.position);
        target.GetComponent<DamageSystem>().TakeDamage(damagePerSecond * Time.deltaTime);
    }

    //private void Act()
    //{
    //    switch (state)
    //    {
    //        case OrderableObjectState.Moving:
    //            agent.isStopped = false;
    //            break;
    //        case OrderableObjectState.Idle:
    //            if (!agent.isStopped)
    //            {
    //                agent.isStopped = true;
    //            }
    //            break;
    //        case OrderableObjectState.Chasing:
    //            agent.isStopped = false;
    //            agent.SetDestination(target.transform.position);
    //            break;
    //        case OrderableObjectState.Attacking:
    //            agent.isStopped = true;
    //            Attack();
    //            break;
    //        case OrderableObjectState.AttackMoving:
    //            agent.isStopped = false;
    //            agent.SetDestination(destination);
    //            UpdateTarget();
    //            break;
    //    }
    //}



    //private void SetState()
    //{
    //    if (gameOrder == null || gameOrder.isPefrormed)
    //    {
    //        if (target == null)
    //        {
    //            state = OrderableObjectState.Idle;
    //        }
    //        else
    //        {
    //            state = OrderableObjectState.Attacking;
    //        }
    //    }
    //    if (gameOrder is MoveTask)
    //    {
    //        if (Vector3.Distance(transform.position, destination) >= reachDistance)
    //        {
    //            state = OrderableObjectState.Moving;
    //        }
    //        else
    //        {
    //            CompleteTask();
    //        }
    //    }
    //    if (gameOrder is AttackTask)
    //    {
    //        if (target != null)
    //        {
    //            if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
    //            {
    //                state = OrderableObjectState.Chasing;
    //            }
    //            else
    //            {
    //                state = OrderableObjectState.Attacking;
    //            }
    //        }
    //        else
    //        {
    //            CompleteTask();
    //        }
    //    }
    //    if (gameOrder is MoveAttackTask)
    //    {
    //        if (target != null)
    //        {
    //            if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
    //            {
    //                state = OrderableObjectState.Chasing;
    //            }
    //            else
    //            {
    //                state = OrderableObjectState.Attacking;
    //            }
    //        }
    //        else
    //        {
    //            if (Vector3.Distance(transform.position, destination) > reachDistance)
    //            {
    //                state = OrderableObjectState.AttackMoving;
    //            }
    //            else
    //            {
    //                CompleteTask();
    //            }
    //        }
    //    }
    //}

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

    public override void UpdateOrder()
    {
        GameOrder order = base.gameOrder;

        if (order is MoveTask)
        {
            if(Vector3.Distance(transform.position, destination) <= reachDistance)
            {
                agent.isStopped = false;
                CompleteTask();
                return;
            }
        }
        if (order is AttackTask)
        {
            if (target == null)
            {
                CompleteTask();
                return;
            }

            if(Vector3.Distance(transform.position, target.transform.position) <= attackDistance)
            {
                Attack();
            }
        }
        if (order is MoveAttackTask)
        {
            if (Vector3.Distance(transform.position, destination) <= reachDistance)
            {
                agent.isStopped = false;
                CompleteTask();
                return;
            }

            if (target == null) UpdateTarget();

            if (Vector3.Distance(transform.position, target.transform.position) <= attackDistance)
            {
                Attack();
            }
        }
    }

    public override void CompleteTask()
    {
        target = null;
        base.CompleteTask();
    }
}
