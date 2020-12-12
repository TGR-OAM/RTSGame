using System;
using System.Diagnostics;
using Assets.Scripts.Orders;
using Assets.Scripts.Orders.Units;
using UnityEngine;

namespace Assets.Scripts.Orders
{
    public enum GameOrderType
    {
        Attack,
        Move,
        MoveAttack,
        Build,
        SpellCast,
        Spawn
    }

    
    [Serializable]
    public class GameOrderInitParams
    {
        public bool IsActiveAtFirst = true;
        public GameOrderType OrderType;
    }
    
    public static class OrderFactory
    {
        public static GameOrder CreateOrder(this GameOrderInitParams orderInitParams)
        {
            switch (orderInitParams)
            {
                case BuildOrderInitParams buildParams:
                    return new BuildOrder(buildParams.building, null);
                default:
                    throw new ArgumentOutOfRangeException(paramName: "orderInitParams");
            }
        }
    }
}
