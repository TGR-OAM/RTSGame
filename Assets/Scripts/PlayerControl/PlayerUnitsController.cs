using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Orders;
using Assets.Scripts.Units;
using Assets.Scripts.UnitsControlScripts;
using UnityEngine;

namespace Assets.Scripts.PlayerControl
{
    public class PlayerUnitsController : MonoBehaviour
    {
        private List<Unit> SelectedUnits;
        private List<Type> PossibleOrdersTypes;

        public void UpdatePossibleOrderTypes()
        {
            List<Type> PossibleOrders = SelectedUnits[0].orderableObject.orderTypes;
            foreach (Unit unit in SelectedUnits)
            {
                PossibleOrders = PossibleOrders.Where(order => unit.orderableObject.orderTypes.Contains(order))
                    .ToList();
            }
        }
        
    }
}