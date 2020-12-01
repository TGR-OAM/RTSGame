using System;
using System.Collections.Generic;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.UnitsControlScripts;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(OrderableObject), typeof(FractionMember))]
    public class Unit : MonoBehaviour
    {
        public float reachDistance { get; protected set; }
        
        public NavMeshAgent agent { get; protected set; }
        public FractionMember fractionMember { get; protected set; }

        private void Start()
        {
            reachDistance = .1f;

            this.transform.GetComponent<OrderableObject>().SetPossibleOrderTypes(new List<Type> {typeof(MoveTask)});
            fractionMember = GetComponent<FractionMember>();
            agent = GetComponent<NavMeshAgent>();
        }
    }
}
