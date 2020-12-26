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

    private bool isFirst = true;
    private float timeFromPreviousCreation;
    [SerializeField]
    private Vector3 creatingPos;
    [SerializeField]
    private Warrior WarriorToCreate;
    [SerializeField]
    private GameObject TargetToAttack;
    
    private void Start()
    {
        //Instantiate(BuilderToCreate, creatingPos, Quaternion.identity);
    }

    void Update()
    {
        timeFromPreviousCreation += Time.deltaTime;
        if (timeFromPreviousCreation >= unitCreatingInterval && !isFirst)
        {

            Warrior warrior = Instantiate(WarriorToCreate, new Vector3(this.transform.position.x,0,transform.position.z), Quaternion.identity);

            if (TargetToAttack != null)
            {
                warrior.orderableObject.GiveOrder((new MoveAttackOrderInitParams("")).CreateOrder(new MoveAttackOrderVariableParams(TargetToAttack.transform.position,warrior.gameObject)));
            }
            
            timeFromPreviousCreation = 0;
        }
        else if(timeFromPreviousCreation >= FirstunitCreatingInterval && isFirst)
        {
            isFirst = false;
            Warrior warrior = Instantiate(WarriorToCreate, new Vector3(this.transform.position.x,0,transform.position.z), Quaternion.identity);

            if (TargetToAttack != null)
            {
                warrior.orderableObject.GiveOrder((new MoveAttackOrderInitParams("")).CreateOrder(new MoveAttackOrderVariableParams(TargetToAttack.transform.position,warrior.gameObject)));
            }
            
            timeFromPreviousCreation = 0;
        }
    }
}
