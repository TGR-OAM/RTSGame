using System.Collections.Generic;
using Buildings;
using ErrorReport;
using MainMenu_DemoStartScripts;
using Units;
using UnitsControlScripts;
using UnityEngine;

namespace Orders.EntityOrder
{
    public class UnitCreationOrderInitParams : GameOrderInitParams
    {
        public Unit unitPrefabToCreate;
        public float TimeToCreateUnit;
        public UnitCreationOrderInitParams(Unit unitPrefabToCreate, float TimeToCreateUnit, string orderName) : base(orderName)
        {
            this.unitPrefabToCreate = unitPrefabToCreate;
            this.TimeToCreateUnit = TimeToCreateUnit;
        }
    }

    public class UnitCreationOrderVariableParams : GameOrderVariableParams
    { 
        public Vector3 positionToMove;

        public UnitCreationOrderVariableParams(Vector3 positionToMove, GameObject objectToOrder) : base(objectToOrder)
        {
            this.positionToMove = positionToMove;
        }
    }
    
    public class UnitCreationOrder:GameOrder
    {
        private Unit unitPrefabToCreate;
        private float TimeUntilConstruction;
        private Vector3 positionToMoveAfterCreation;
        private GameObject ObjectToCreate;

        public UnitCreationOrder(UnitCreationOrderInitParams unitCreationOrderInitParams ,UnitCreationOrderVariableParams unitCreationOrderVariableParams) : base(unitCreationOrderVariableParams)
        {
            unitPrefabToCreate = unitCreationOrderInitParams.unitPrefabToCreate;
            TimeUntilConstruction = unitCreationOrderInitParams.TimeToCreateUnit;
            positionToMoveAfterCreation = unitCreationOrderVariableParams.positionToMove;
            ObjectToCreate = unitCreationOrderVariableParams.ObjectToOrder;
        }

        public override void StartOrder()
        {
            base.StartOrder();
        }

        public override void UpdateOrder()
        {
            TimeUntilConstruction -= Time.deltaTime;
            
            if (TimeUntilConstruction <= 0)
            {
                CreateUnitFromParts();
                StopOrder();
            }
        }

        public override void StopOrder()
        {
            base.StopOrder();
        }

        void CreateUnitFromParts()
        {
            Unit createdUnit = GameObject.Instantiate(unitPrefabToCreate);
            createdUnit.agent.Warp(ObjectToOrder.transform.position + new Vector3(ObjectToOrder.transform.localScale.x/2,0,0));

            string MoveOrderKeyValue = typeof(MoveOrderInitParams).FullName;
            
            MoveOrderInitParams moveOrderInitParams = EntityLoader
                .GetOrderInitParamsFromDictionary(createdUnit.GetType()).GetOrderInitParamsFromType(MoveOrderKeyValue) as MoveOrderInitParams;
            createdUnit.orderableObject.GiveOrder(moveOrderInitParams.CreateOrder(new MoveOrderVariableParams(positionToMoveAfterCreation,createdUnit.gameObject)));
        }
    }
}