using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class MoveTask : GameOrder
    {
        private Vector3 destination;

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
                thisUnit.agent.SetDestination(destination);
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
                unit.agent.SetDestination(unit.transform.position);
                return;
            }
        }
    }
}