﻿using Assets.Scripts.Units;
using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class BuildTask : GameOrder
    {
        private Vector3 destination;
        private Building buildingToBuild;

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
                Unit unit = component as Unit;
                unit.agent.SetDestination(destination);
            }
        }

        public override void UpdateOrder()
        {
            if (ObjectToOrder.TryGetComponent(typeof(UnitBuilder), out Component component))
            {
                UnitBuilder unit = component as UnitBuilder;
                if (buildingToBuild == null) StopOrder();
                if (buildingToBuild.timeUntilConstruction <= 0) StopOrder();
                if (Vector3.Distance(unit.transform.position, destination) <= unit.reachDistance)
                {
                    isBuilding = true;
                    unit.agent.isStopped = true;
                }

                if(isBuilding)
                    buildingToBuild.timeUntilConstruction -= Time.deltaTime;
            }
        }

        public override void StopOrder()
        {
            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                Unit unit = component as Unit;
                unit.agent.isStopped = true;
                return;
            }
        }
    }
}