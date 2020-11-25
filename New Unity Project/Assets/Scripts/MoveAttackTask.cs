using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAttackTask : GameOrder
{
    public Vector3 destination { get; private set; }
    public MoveAttackTask(Vector3 destination)
    {
        this.destination = destination;
    }
}
