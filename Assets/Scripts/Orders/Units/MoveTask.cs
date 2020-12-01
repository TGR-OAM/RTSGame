using Assets.Scripts.UnitsControlScripts;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class MoveTask : GameOrder
    {
        public Vector3 destination { get; private set; }

        public MoveTask(Vector3 d, GameObject ObjectToOrder) : base(ObjectToOrder)
        {
            destination = d;
        }

        public override void StartOrder()
        {
            base.StartOrder();

            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                Unit unit = component as Unit;
                unit.agent.SetDestination(destination);
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