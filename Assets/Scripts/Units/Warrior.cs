using System;
using System.Collections.Generic;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.UnitsControlScripts;
using UnityEngine;
using UnityEngine.AI;

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
            
            attackDistance = 20f;
            visionDistance = 10f;
            damagePerSecond = 10f;

            #endregion
            
            BaseUnitInitialization();
            
            orderableObject.SetPossibleOrderTypes(new List<Type> {typeof(MoveOrder),typeof(AttackOrder),typeof(MoveAttackOrder)});
        }
        
    }
}