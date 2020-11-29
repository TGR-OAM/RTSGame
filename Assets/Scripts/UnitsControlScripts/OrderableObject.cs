using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrderableObjectState
{
    Attacking, //воюет
    Moving, //убегает
    Chasing, //преследует врага
    Building, //строит
    Gathering, //собирает
    AttackMoving,//двигается и смотрит, кого бы убить, как ЛКМ+А в Старкрафте
    Defending,//защищается
    Idle,//бездействует
}

public class OrderableObject : MonoBehaviour
{
    [SerializeField]
    public GameOrder gameOrder { get; private set; }
    public OrderableObjectState state = OrderableObjectState.Defending;

    public List<Type> orderTypes { get;  protected set; } 

    public virtual void GiveOrder(GameOrder order)
    {
        CompleteTask();
        gameOrder = order;
    }

    public virtual void UpdateOrder()
    {
    }

    public virtual void CompleteTask()
    {
        if (gameOrder != null)
        {
            gameOrder.isPefrormed = true;
        }
        state = OrderableObjectState.Idle;
    }
}
