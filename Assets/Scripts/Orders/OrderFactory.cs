using System;
using System.Diagnostics;
using Orders.EntityOrder;
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
        Spawn,
        None
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
                    return new MoveOrder(moveParams.destination);
                case MoveAttackOrderInitParams moveAtackParams:
                    return new MoveAttackOrder(moveAtackParams.destination);
                default:
                    throw new ArgumentOutOfRangeException(paramName: "orderInitParams");
            }
        }
    }
}
