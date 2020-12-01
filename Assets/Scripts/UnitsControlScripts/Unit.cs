using System;
using System.Collections.Generic;
using Assets.Scripts.Orders.Units;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.UnitsControlScripts
{
    [RequireComponent(typeof(OrderableObject), typeof(FractionMember))]
    public class Unit : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; }
        public float reachDistance { get; private set; }
        public float attackDistance { get; private set; }
        public float visionDistance { get; private set; }
        public float damagePerSecond { get; private set; }
        public FractionMember fractionMember { get; private set; }

        private void Start()
        {
            this.transform.GetComponent<OrderableObject>().SetPossibleOrderTypes(new List<Type>
                {typeof(MoveAttackTask), typeof(MoveTask), typeof(AttackTask)});
            fractionMember = GetComponent<FractionMember>();
            agent = GetComponent<NavMeshAgent>();
        }
    }
}
