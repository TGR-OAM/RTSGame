using Assets.Scripts.Units;
using Assets.Scripts.Buildings;
using Assets.Scripts.UnitsControlScripts;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class BuildTask : GameOrder
    {
        private Vector3 destination;
        private DamageSystem buildingDamageSystem;

        private float TimeUntilFullConstruction;
        
        private Unit unitToOrder;
        
        public BuildTask(Vector3 destination,Building buildingToBuild, GameObject ObjectToOrder) : base(ObjectToOrder)
        {
            this.destination = destination;
            this.buildingDamageSystem = buildingToBuild.damageSystem;
            this.TimeUntilFullConstruction = buildingToBuild.TimeUntilFullConstruction;
        }

        public override void StartOrder()
        {
            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                base.StartOrder();
                
                unitToOrder = component as Unit;
                unitToOrder.agent.isStopped = false;
                unitToOrder.agent.SetDestination(destination);
            }
        }

        public override void UpdateOrder()
        {
            if (unitToOrder != null)
            {
                if (buildingDamageSystem == null) StopOrder();
                if (unitToOrder.isNearToDestination(destination,unitToOrder.reachDistance))
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
                unitToOrder.agent.SetDestination(unitToOrder.transform.position);
            }
        }
    }
}