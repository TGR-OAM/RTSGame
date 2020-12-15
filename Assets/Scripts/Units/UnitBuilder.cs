﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.UnitsControlScripts;

namespace Assets.Scripts.Units
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