using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using MainMenu_DemoStartScripts;
using Orders;
using Orders.EntityOrder;
using UnitsControlScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(OrderableObject), typeof(FractionMember), typeof(DamageSystem))]
    public class Unit : MonoBehaviour
    {
        public float reachDistance;

        public float MaxHp;

        public NavMeshAgent agent;
        public FractionMember fractionMember;
        public OrderableObject orderableObject;
        
        public DamageSystem damageSystem { get; private set; }

        protected void Start()
        {
            reachDistance = .1f;

            BaseUnitInitialization();
            BaseOrderListInitialization();
        }

        protected void BaseUnitInitialization()
        {
            orderableObject = GetComponent<OrderableObject>();
            fractionMember = GetComponent<FractionMember>();
            agent = GetComponent<NavMeshAgent>();
            damageSystem = GetComponent<DamageSystem>();
            damageSystem.SetMaxHp(MaxHp);
            damageSystem.SetHpToMax();
        }

        protected void BaseOrderListInitialization()
        {
            if (EntityLoader.Contain(this.GetType()))
            {
                orderableObject.SetPossibleOrderTypes(EntityLoader.GetOrderInitParamsFromDictionary(this.GetType())
                    .OrderInitParams.Values.ToList());
            }
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
