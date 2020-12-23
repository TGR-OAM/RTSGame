using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orders.EntityOrder
{
    [Serializable]
    public class GameOrderInitParams
    {
    }
    
    public class GameOrderVariableParams
    {
        public GameObject ObjectToOrder;

        public GameOrderVariableParams(GameObject objectToOrder)
        {
            ObjectToOrder = objectToOrder;
        }
    }

    [Serializable]
    public class GameOrder{
        public bool isPefrorming { get; private set; } = false;
        public GameObject ObjectToOrder;
        public bool isAvailable { get; set; } = true;

        public GameOrder(GameOrderVariableParams gameOrderVariableParams)
        {
            ObjectToOrder = gameOrderVariableParams.ObjectToOrder;
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