using System;
using System.Collections.Generic;
using Orders.Units;

namespace Units
{
    public class UnitBuilder:Unit
    {
        
        
        private void Start()
        {
            BaseUnitInitialization();

            orderableObject.SetPossibleOrderTypes(new List<Type>
                {typeof(MoveOrder), typeof(BuildOrder)});
        }
    }
}