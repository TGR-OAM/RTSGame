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
        public List<Unit> SelectedUnits;
        
        private List<Type> PossibleOrdersTypes;

        #region CahsedData

        private List<Unit> CachedSelectedUnits;

        #endregion

        public void UpdatePossibleOrderTypes()
        {
            if (SelectedUnits.Count == 0)
            {
                SelectedUnits = new List<Unit>();
                return;
            }
            
            List<Type> PossibleOrders = SelectedUnits[0].orderableObject.orderTypes;
            foreach (Unit unit in SelectedUnits)
            {
                PossibleOrders = PossibleOrders.Where(order => unit.orderableObject.orderTypes.Contains(order))
                    .ToList();
            }
        }

        private void Update()
        {
            if (SelectedUnits != CachedSelectedUnits)
            {
                UpdatePossibleOrderTypes();
                CachedSelectedUnits = SelectedUnits;
            }
        }
    }
}