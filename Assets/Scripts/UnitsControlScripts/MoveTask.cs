using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTask : GameOrder
{
    public Vector3 destination { get; private set; }
    public MoveTask (Vector3 d, Unit unit)
    {
        destination = d;
    }

    //example
    public override void StartOrder(object OrderableObject)
    {
        if(OrderableObject is Unit)
        {
            base.isPefrormed = true;
            (OrderableObject as Unit).agent.SetDestination(destination);
        }
    }

     
}
