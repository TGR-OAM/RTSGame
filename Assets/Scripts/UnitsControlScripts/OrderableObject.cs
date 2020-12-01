using System;
using System.Collections.Generic;
using Assets.Scripts.Orders;
using UnityEngine;

namespace Assets.Scripts.UnitsControlScripts
{
    public class OrderableObject : MonoBehaviour
    {
        public GameOrder gameOrder { get; protected set; }
        public List<Type> orderTypes;

        public void SetPossibleOrderTypes(List<Type> orderTypes)
        {
            this.orderTypes = orderTypes;
        }

        public virtual void GiveOrder(GameOrder order)
        {
            CompleteTask();
            gameOrder = order;
        }
    
        public virtual void CompleteTask()
        {
            if (gameOrder != null)
            {
                gameOrder.StopOrder();
            }
        }
    }
}
