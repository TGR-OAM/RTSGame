using Buildings;
using Units;
using UnitsControlScripts;
using UnityEngine;

namespace Orders.EntityOrder
{
    public class BuildOrderInitParams : GameOrderInitParams
    {
        public Building building;

        public BuildOrderInitParams(Building building, string orderName) : base(orderName)
        {
            this.building = building;
        }
    }

    public class BuildOrderVariableParams : GameOrderVariableParams
    {
        public Building buildingOnMapToBuild;
        
        public BuildOrderVariableParams(Building buildingOnMapToBuild,GameObject objectToOrder) : base(objectToOrder)
        {
            this.buildingOnMapToBuild = buildingOnMapToBuild;
        }
    }
    
    public class BuildOrder : GameOrder
    {
        private DamageSystem buildingDamageSystem;
        private Building BuildingToBuild;

        private float TimeUntilFullConstruction;
        private float NearestDistance;
        
        private Unit unitToOrder;
        
        public BuildOrder(BuildOrderVariableParams buildOrderVariableParams) : base (buildOrderVariableParams)
        {
            this.BuildingToBuild = buildOrderVariableParams.buildingOnMapToBuild;
            this.buildingDamageSystem = BuildingToBuild.damageSystem;
            this.TimeUntilFullConstruction = BuildingToBuild.TimeUntilFullConstruction;
        }

        public override void StartOrder()
        {
            if (BuildingToBuild!= null && ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                base.StartOrder();
                
                unitToOrder = component as Unit;
                unitToOrder.agent.isStopped = false;
                unitToOrder.agent.SetDestination(BuildingToBuild.ObjectCollider.bounds.center);

                NearestDistance = BuildingToBuild.ObjectCollider.bounds.extents.magnitude + 1f;
            }
        }

        public override void UpdateOrder()
        {
            if (unitToOrder != null)
            {
                if (buildingDamageSystem == null) StopOrder();
                if (unitToOrder.isNearToDestination(BuildingToBuild.ObjectCollider.bounds.center,NearestDistance))
                {
                    buildingDamageSystem.Heal(buildingDamageSystem.MaxHp/TimeUntilFullConstruction * Time.deltaTime);
                    unitToOrder.agent.SetDestination(unitToOrder.transform.position);
                }
                if (buildingDamageSystem.Hp >= buildingDamageSystem.MaxHp) StopOrder();
            }
        }

        public override void StopOrder()
        {
            base.StopOrder();
            
            if (unitToOrder != null)
            {
                unitToOrder.agent.isStopped = true;
            }
        }
    }
}