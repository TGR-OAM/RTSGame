using System;
using System.Collections.Generic;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.UnitsControlScripts;

namespace Assets.Scripts.Units
{
    public class Builder:Unit
    {
        
        
        private void Start()
        {
            this.GetComponent<OrderableObject>().SetPossibleOrderTypes(new List<Type>
                {typeof(MoveTask), });
        }
    }
}