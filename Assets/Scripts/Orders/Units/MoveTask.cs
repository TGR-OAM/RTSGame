using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class MoveTask : GameOrder
    {
        private Vector3 destination;
        private Unit UnitToOrder;

        public MoveTask(Vector3 destination ,GameObject ObjectToOrder) : base(ObjectToOrder)
        {
            this.destination = destination;
        }

        public override void StartOrder()
        {
            base.StartOrder();

            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                Unit thisUnit = component as Unit;
                UnitToOrder = thisUnit;
                thisUnit.agent.SetDestination(destination);
            }
        }

        public override void UpdateOrder()
        {
            if (UnitToOrder != null)
            {
                if (Vector3.Distance(UnitToOrder.transform.position, destination) <= UnitToOrder.reachDistance)
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
                UnitToOrder.agent.SetDestination(UnitToOrder.transform.position);
            }
        }
    }
}