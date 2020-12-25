using System;
using System.Diagnostics;
using Orders.EntityOrder;
using UnityEngine;

namespace Orders
{
    // public enum GameOrderType
    // {
    //     Attack,
    //     Move,
    //     MoveAttack,
    //     Build,
    //     SpellCast,
    //     Spawn,
    //     None
    // }

    public static class OrderFactory
    {
        public static GameOrder CreateOrder(this GameOrderInitParams orderInitParams, GameOrderVariableParams orderVariableParams)
        {
            switch (orderInitParams)
            {
                case BuildOrderInitParams buildParams:
                    return new BuildOrder(orderVariableParams as BuildOrderVariableParams);
                case AttackOrderInitParams attackParams:
                    return new AttackOrder(orderVariableParams as AttackOrderVariableParams);
                case MoveOrderInitParams moveParams:
                    return new MoveOrder(orderVariableParams as MoveOrderVariableParams);
                case MoveAttackOrderInitParams moveAtackParams:
                    return new MoveAttackOrder(orderVariableParams as MoveAttackOrderVariableParams);
                case UnitCreationOrderInitParams unitCreationOrderInitParams:
                    return new UnitCreationOrder(unitCreationOrderInitParams,
                        orderVariableParams as UnitCreationOrderVariableParams);
                default:
                    throw new ArgumentOutOfRangeException(paramName: "orderInitParams");
            }
        }
    }
}
