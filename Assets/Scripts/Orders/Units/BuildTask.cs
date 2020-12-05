using Assets.Scripts.Units;
using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class BuildTask : GameOrder
    {
        private Vector3 destination;
        private Building buildingToBuild;

        private Unit UnitToOrder;
        
        private bool isBuilding = false;

        public BuildTask(Vector3 destination,Building buildingToBuild,GameObject ObjectToOrder) : base(ObjectToOrder)
        {
            this.destination = destination;
            this.buildingToBuild = buildingToBuild;
        }

        public override void StartOrder()
        {
            base.StartOrder();

            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                UnitToOrder = component as Unit;
                UnitToOrder.agent.SetDestination(destination);
            }
        }
        
        
        
        public override void UpdateOrder()
        {
            if (UnitToOrder != null)
            {
                if (buildingToBuild == null) StopOrder();
                if (buildingToBuild.timeUntilConstruction <= 0) StopOrder();
                if (Vector3.Distance(UnitToOrder.transform.position, destination) <= UnitToOrder.reachDistance)
                {
                    isBuilding = true;
                    UnitToOrder.agent.isStopped = true;
                }

                if(isBuilding)
                    buildingToBuild.timeUntilConstruction -= Time.deltaTime;
            }
        }

        public override void StopOrder()
        {
            if (UnitToOrder != null)
            {
                UnitToOrder.agent.isStopped = true;
                return;
            }
        }
    }
}