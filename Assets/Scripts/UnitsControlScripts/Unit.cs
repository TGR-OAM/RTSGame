using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour //@author
{
    [SerializeField]
    public GameOrder gameOrder { get; private set; }
    public UnitState state = UnitState.Idle;
    public float reachDistance = 1f;
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