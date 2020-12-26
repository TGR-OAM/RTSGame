using System;
using System.Collections.Generic;
using Orders;
using Orders.EntityOrder;


namespace Units
{
    public class Warrior: Unit
    {
        public float attackrecharge;
        public float attackDistance { get; protected set; }
        public float visionDistance { get; protected set; }
        public float damagePerSecond { get; protected set; }

        protected void Start()
        {
            base.Start();
            
        }
        
    }
}