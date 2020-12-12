using System;
using UnityEngine;

namespace Assets.Scripts.Orders
{
    [Serializable]
    public class GameOrder{
        public bool isPefrorming { get; private set; } = false;
        public GameObject ObjectToOrder;
        public bool isAvailable { get; set; } = true;

        public GameOrder(GameObject ObjectToOrder)
        {
            this.ObjectToOrder = ObjectToOrder;
        }

        public virtual void StartOrder()
        {
            isPefrorming = true;
        }

        public virtual void UpdateOrder()
        { }

        public virtual void StopOrder()
        {
            isPefrorming = false;
        }
    }
}