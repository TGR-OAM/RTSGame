using System;
using System.Collections.Generic;
using Orders;
using Orders.EntityOrder;


namespace Units
{
    public class Warrior: Unit
    {
        public float attackrecharge;
        public float attackDistance = 10f;
        public float visionDistance = 20f;
        public float damagePerSecond;

        protected void Start()
        {
            base.Start();
            
        }
        
    }
}