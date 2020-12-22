using System;
using System.Collections.Generic;
using Orders;

namespace Units
{
    public class UnitBuilder:Unit
    {
        
        
        private void Start()
        {
            BaseUnitInitialization();

            orderableObject.SetPossibleOrderTypes(new List<GameOrderType>
                {GameOrderType.Move,GameOrderType.Build});
        }
    }
}