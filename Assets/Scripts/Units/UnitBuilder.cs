using System;
using System.Collections.Generic;
using Buildings;
using Orders;
using Orders.EntityOrder;

namespace Units
{
    public class UnitBuilder:Unit
    {
        public Building[] buildings;
        
        private void Start()
        {
            BaseUnitInitialization();

            List<GameOrderInitParams> initParams = new List<GameOrderInitParams>
            {
                new MoveOrderInitParams(GameOrderType.Move),
            };

            foreach (Building building in buildings)
            {
                initParams.Add(new BuildOrderInitParams(building,GameOrderType.Build));
            }
            
            orderableObject.SetPossibleOrderTypes(initParams);
        }
    }
}