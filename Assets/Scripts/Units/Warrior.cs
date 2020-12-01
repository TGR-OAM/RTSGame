using System;
using System.Collections.Generic;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.UnitsControlScripts;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Scripts.Units
{
    public class Warrior: Unit
    {
        public float attackDistance { get; protected set; }
        public float visionDistance { get; protected set; }
        public float damagePerSecond { get; protected set; }

        private void Start()
        {
            #region Part-time data
            
            attackDistance = .1f;
            visionDistance = 10f;
            damagePerSecond = 100f;

            #endregion
            
            this.GetComponent<OrderableObject>().SetPossibleOrderTypes(new List<Type> {typeof(MoveTask),typeof(AttackTask),typeof(MoveAttackTask)});
            fractionMember = GetComponent<FractionMember>();
            agent = GetComponent<NavMeshAgent>();
        }
        
    }
}