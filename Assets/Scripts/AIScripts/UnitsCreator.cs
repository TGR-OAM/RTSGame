using System;
using System.Collections;
using System.Collections.Generic;
using Orders;
using Orders.EntityOrder;
using Units;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitsCreator : MonoBehaviour
{
    [SerializeField]
    private float unitCreatingInterval;
    [SerializeField]
    private float FirstunitCreatingInterval;
    
    [SerializeField]
    private float SecondtunitCreatingInterval;

    private int NumPhase = 0;
    private float timeFromPreviousCreation;
    [SerializeField]
    private Vector3 creatingPos;
    [SerializeField]
    private Warrior WarriorToCreate;
    [SerializeField]
    private GameObject TargetToAttack;
    [SerializeField]
    private GameObject DefendToAttack;
    
    private void Start()
    {
        //Instantiate(BuilderToCreate, creatingPos, Quaternion.identity);
    }

    void Update()
    {
        timeFromPreviousCreation += Time.deltaTime;
        if (timeFromPreviousCreation >= FirstunitCreatingInterval && NumPhase < 6)
        {
            NumPhase++;
            Warrior warrior = Instantiate(WarriorToCreate, new Vector3(this.transform.position.x,0,transform.position.z), Quaternion.identity);

            if (TargetToAttack != null)
            {
                warrior.orderableObject.GiveOrder((new DefendOrderInitParams("")).CreateOrder(new DefendOrderVariableParams(DefendToAttack.transform.position,warrior.gameObject)));
            }
            
            timeFromPreviousCreation = 0;
        }
        else if(timeFromPreviousCreation >= unitCreatingInterval && NumPhase >= 6)
        {
            NumPhase ++;
            Warrior warrior = Instantiate(WarriorToCreate, new Vector3(this.transform.position.x,0,transform.position.z), Quaternion.identity);

            if (TargetToAttack != null)
            {
                warrior.orderableObject.GiveOrder((new MoveAttackOrderInitParams("")).CreateOrder(new MoveAttackOrderVariableParams(TargetToAttack.transform.position,warrior.gameObject)));
            }
            
            timeFromPreviousCreation = 0;
        }

    }
}
