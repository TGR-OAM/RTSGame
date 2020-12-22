using System;
using System.Collections.Generic;
using Orders;
using Orders.Units;
using UnitsControlScripts;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(OrderableObject), typeof(FractionMember), typeof(DamageSystem))]
    public class Unit : MonoBehaviour
    {
        public float reachDistance { get; protected set; }

        public float MaxHp;
        
        public NavMeshAgent agent { get; protected set; }
        public FractionMember fractionMember { get; protected set; }
        public OrderableObject orderableObject { get; private set; }

        public DamageSystem damageSystem { get; private set; }

        private void Start()
        {
            reachDistance = .1f;

            BaseUnitInitialization();
            
            orderableObject.SetPossibleOrderTypes(new List<GameOrderType> {GameOrderType.Move});
        }

        protected void BaseUnitInitialization()
        {
            orderableObject = GetComponent<OrderableObject>();
            fractionMember = GetComponent<FractionMember>();
            agent = GetComponent<NavMeshAgent>();
            damageSystem = GetComponent<DamageSystem>();
            damageSystem.SetMaxHpd(MaxHp);
            damageSystem.SetHpToMax();
        }


        public bool isNearToDestination(Vector3 destination, float distance)
        {
            if (Vector3.Distance(this.transform.position, destination) <= distance)
                return true;
            else
                return false;
        }
    }
}
