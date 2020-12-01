using System;
using System.Collections.Generic;
using Assets.Scripts.Orders;
using UnityEngine;

namespace Assets.Scripts.UnitsControlScripts
{
    public class OrderableObject : MonoBehaviour
    {
        public GameOrder currentOrder { get; set; }
        public List<Type> orderTypes{ get; protected set; }

        public void SetPossibleOrderTypes(List<Type> orderTypes)
        {
            this.orderTypes = orderTypes;
        }

        public void GiveOrder(GameOrder order)
        {
            CompleteTask();
            currentOrder = order;
        }

        private void Update()
        {
            if (currentOrder != null)

            {
                if (currentOrder.isPefrormed)
                {
                    currentOrder.UpdateOrder();
                }
            }
        }

        public void CompleteTask()
        {
            if (currentOrder != null)
            {
                currentOrder.StopOrder();
            }
        }
    }
}
