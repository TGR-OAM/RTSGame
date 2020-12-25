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

        protected void Start()
        {
            base.Start();
            
            #region Part-time data
            
            attackDistance = 20f;
            visionDistance = 30f;
            damagePerSecond = 10f;

            #endregion
        }
        
    }
}