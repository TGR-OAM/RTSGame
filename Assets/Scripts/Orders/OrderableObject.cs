using System;
using System.Collections.Generic;
using System.Linq;
using Orders;
using Orders.EntityOrder;
using UnityEngine;

namespace UnitsControlScripts
{
    public class OrderableObject : MonoBehaviour
    {
        public GameOrder currentOrder { get; set; }
        private GameOrder[] ourOrders;// для списка приказов, забить до 26.12 числа точно
        [SerializeField] public GameOrderInitParams[] GameOrderInitParamsArray;

        public void SetPossibleOrderTypes(List<GameOrderInitParams> orderTypes)
        {
            GameOrderInitParamsArray = orderTypes.ToArray();
        }

        public void GiveOrder(GameOrder order)
        {
            CompleteTask();
            currentOrder = order;
            order.ObjectToOrder = this.gameObject;
            currentOrder.StartOrder();
        }

        private void Update()
        {
            if (currentOrder != null)
            {
                if (currentOrder.isPefrorming)
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
