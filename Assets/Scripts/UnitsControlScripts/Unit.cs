using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    Attacking, //воюет
    Moving, //убегает
    Chasing, //преследует врага
    Building, //строит
    Gathering, //собирает
    AttackMoving,//двигается и смотрит, кого бы убить, как ЛКМ+А в Старкрафте
    Defending,//защищается
    Idle//бездействует
}

public class Unit : MonoBehaviour //@author
{
    [SerializeField]
    public GameOrder gameOrder { get; private set; }
    public UnitState state = UnitState.Defending;

    [SerializeField]
    protected float reachDistance = 1f;
    public List<Type> orderTypes { get;  protected set; } 
    public virtual void GiveOrder(GameOrder order)
    {
        CompleteTask();
        gameOrder = order;
    }
    public virtual void CompleteTask()
    {
        if (gameOrder != null)
        {
            gameOrder.isPefrormed = true;
        }
        state = UnitState.Idle;
    }
}
