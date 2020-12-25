using System.ComponentModel;
using Units;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Orders.EntityOrder
{
    public class MoveOrderInitParams : GameOrderInitParams
    {
        public MoveOrderInitParams(string orderName) : base(orderName)
        {
        }
    }
    
    public class MoveOrderVariableParams : GameOrderVariableParams
    {
        public Vector3 destination;
        
        public MoveOrderVariableParams(Vector3 destination, GameObject objectToOrder) : base(objectToOrder)
        {
            this.destination = destination;
        }
    }
    
    public class MoveOrder : GameOrder
    {
        private Vector3 destination;
        private Unit UnitToOrder;

        public MoveOrder(MoveOrderVariableParams orderVariableParams) :base(orderVariableParams)
        {
            this.destination = orderVariableParams.destination;
        }

        public override void StartOrder()
        {
            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                base.StartOrder();
                Unit thisUnit = component as Unit;
                UnitToOrder = thisUnit;
                Debug.Log(UnitToOrder);
                UnitToOrder.agent.isStopped = false;
                UnitToOrder.agent.SetDestination(destination);
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