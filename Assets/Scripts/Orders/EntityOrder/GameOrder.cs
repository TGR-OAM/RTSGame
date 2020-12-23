using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orders.EntityOrder
{
    [Serializable]
    public class GameOrderInitParams
    {
        public GameOrderType OrderType;
        public GameOrderInitParams(GameOrderType type)
        {
            OrderType = type;
        }
    }

    [Serializable]
    public class GameOrder{
        public bool isPefrorming { get; private set; } = false;
        public GameObject ObjectToOrder;
        public bool isAvailable { get; set; } = true;
        public List<GameOrderType> orderType { get; protected set; }

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