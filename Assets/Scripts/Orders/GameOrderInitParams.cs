using System;
using System.Diagnostics;
using Orders.Units;
using UnityEngine;

namespace Orders
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
                    return new BuildOrder(buildParams.building);
                case AttackOrderInitParams attackParams:
                    return new AttackOrder(attackParams.target);
                case MoveOrderInitParams moveParams:
                    if (moveParams.isForMoveAttackOrder)
                    {
                        return new MoveAttackOrder(moveParams.destination);
                    } else
                    {
                        return new MoveOrder(moveParams.destination);
                    }
                default:
                    throw new ArgumentOutOfRangeException(paramName: "orderInitParams");
            }
        }
    }
}
