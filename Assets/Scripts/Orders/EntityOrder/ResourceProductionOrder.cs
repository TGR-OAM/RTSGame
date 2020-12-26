using GameResources;
using PLayerScripts;
using UnitsControlScripts;
using UnityEngine;

namespace Orders.EntityOrder//decided to be useless
{
    public class ResourceProductionOrderInitParams:GameOrderInitParams
    {
        public GameResourceStruct gameResourceStruct;
        public float TimeToProduce;
        
        public ResourceProductionOrderInitParams(GameResourceStruct gameResourceStruct,float TimeToProduce, string orderName) : base(orderName)
        {
            this.gameResourceStruct = gameResourceStruct;
            this.TimeToProduce = TimeToProduce;
        }
    }

    public class ResourceProductionOrderVariableParams : GameOrderVariableParams
    {
        public PlayerResoucesManager ResoucesManager;

        public ResourceProductionOrderVariableParams(PlayerResoucesManager resoucesManager,GameObject objectToOrder) : base(objectToOrder)
        {
            ResoucesManager = resoucesManager;
        }
    }

    public class ResourceProductionOrder : GameOrder
    {
        private float TimeUntilProduction;
        public GameResourceStruct gameResourceStruct;

        private ResourceProductionOrderInitParams gameOrderInitParams;
        private ResourceProductionOrderVariableParams gameOrderVariableParams;

        public ResourceProductionOrder(ResourceProductionOrderInitParams gameOrderInitParams,
            ResourceProductionOrderVariableParams gameOrderVariableParams) : base(gameOrderVariableParams)
        {
            this.gameOrderInitParams = gameOrderInitParams;
            TimeUntilProduction = gameOrderInitParams.TimeToProduce;
            gameResourceStruct = gameOrderInitParams.gameResourceStruct;
        }

        public override void StartOrder()
        {
            base.StartOrder();
        }

        public override void UpdateOrder()
        {
            TimeUntilProduction -= Time.deltaTime;

            if (TimeUntilProduction < 0)
                StopOrder();
        }

        public override void StopOrder()
        {
            if (TimeUntilProduction < 0)
            {
                ResourceProductionOrder resourceProductionOrder =
                    gameOrderInitParams.CreateOrder(gameOrderVariableParams) as ResourceProductionOrder;
                ObjectToOrder.GetComponent<OrderableObject>().GiveOrder(resourceProductionOrder);
            }

            base.StopOrder();
        }
    }
}