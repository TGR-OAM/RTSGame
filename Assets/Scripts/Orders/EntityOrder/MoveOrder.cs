using System.ComponentModel;
using Units;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Orders.EntityOrder
{
    public class MoveOrderInitParams : GameOrderInitParams
    {
        public Vector3 destination;
        public MoveOrderInitParams(GameOrderType type) : base(type)
        {
        }
    }
    
    public class MoveOrder : GameOrder
    {
        private Vector3 destination;
        private Unit UnitToOrder;

        public MoveOrder(Vector3 destination)
        {
            this.destination = destination;
        }

        public override void StartOrder()
        {
            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                base.StartOrder();
                Unit thisUnit = component as Unit;
                UnitToOrder = thisUnit;
                UnitToOrder.agent.isStopped = false;
                thisUnit.agent.SetDestination(destination);
            }
        }

        public override void UpdateOrder()
        {
            if (UnitToOrder != null)
            {
                if (UnitToOrder.isNearToDestination(destination,UnitToOrder.reachDistance))
                {
                    StopOrder();
                }
            }
        }

        public override void StopOrder()
        {
            base.StopOrder();
            
            if (UnitToOrder != null)
            {
                UnitToOrder.agent.isStopped = true;
            }
        }
    }
}