using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask : GameOrder
{
    public GameObject target;
    public AttackTask (GameObject t)
    {
        target = t;
    }
}