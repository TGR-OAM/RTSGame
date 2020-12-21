using System;
using System.Collections.Generic;
using System.Linq;
using Orders;
using UnityEngine;

namespace UnitsControlScripts
{
    public class OrderableObject : MonoBehaviour
    {
        public GameOrder currentOrder { get; set; }
        public List<Type> orderTypes { get; protected set; } = new List<Type>();
        private GameOrder[] ourOrders;// для списка приказов, забить до 26.12 числа точно
        [SerializeField] public GameOrderInitParams[] GameOrderInitParamsArray;

        private void Start()
        {
            InitOrdersArray(GameOrderInitParamsArray);
        }

        private void InitOrdersArray(GameOrderInitParams[] gameOrderInitParamsArray)
        {
            ourOrders = gameOrderInitParamsArray.Select(x => x.CreateOrder()).ToArray();
        }

        public void SetPossibleOrderTypes(List<Type> orderTypes)
        {
            this.orderTypes = orderTypes;
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
