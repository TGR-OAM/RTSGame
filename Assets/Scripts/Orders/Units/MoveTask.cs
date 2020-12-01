﻿using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class MoveTask : GameOrder
    {
        private Vector3 destination;
        private int numOfUnits;

        public MoveTask(Vector3 destination, int numOfUnits,GameObject ObjectToOrder) : base(ObjectToOrder)
        {
            this.destination = destination;
            this.numOfUnits = numOfUnits;
        }

        public override void StartOrder()
        {
            base.StartOrder();

            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                Unit unit = component as Unit;
                unit.agent.SetDestination(destination + new Vector3(Random.value*0.21f,0,Random.value*0.21f)*numOfUnits);
            }
        }

        public override void UpdateOrder()
        {
            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                Unit unit = component as Unit;
                if (Vector3.Distance(unit.transform.position, destination) <= unit.reachDistance)
                {
                    StopOrder();
                    return;
                }
            }
        }

        public override void StopOrder()
        {
            base.StopOrder();
            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                Unit unit = component as Unit;
                unit.agent.isStopped = true;
                return;
            }
        }
    }
}