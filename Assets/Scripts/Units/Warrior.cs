using System;
using System.Collections.Generic;
using Orders;
using Orders.EntityOrder;


namespace Units
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
            
            orderableObject.SetPossibleOrderTypes(new List<GameOrderInitParams>
            {
                new MoveOrderInitParams(),
                new AttackOrderInitParams(),
                new MoveAttackOrderInitParams()
            });
        }
        
    }
}