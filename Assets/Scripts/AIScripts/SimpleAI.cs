using Assets.Scripts.HexWorldinterpretation;
using Assets.Scripts.UnitsControlScripts;
using Assets.Scripts.UnitsControlScripts.UnitsControlScripts;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Orders.Units;
using UnityEngine.SceneManagement;

public class SimpleAI : MonoBehaviour
{
    [SerializeField]
    private HexGrid hexGrid;
    [SerializeField]
    private EntetiesLister unitLister;
    [SerializeField]
    private float attackInterval;
    private float currentTimeFromAttack;
    [SerializeField]
    private Vector3 enemyBasePosition;

    private void Update()
    {
        currentTimeFromAttack += Time.deltaTime;
        if (currentTimeFromAttack >= attackInterval)
        {
            AttackPlayer();
            currentTimeFromAttack = 0;
        }
    }
    
    private void AttackPlayer()
    {
        print("Go go go");
        MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams();
        moveOrderInitParams.destination = enemyBasePosition;
        moveOrderInitParams.isForMoveAttackOrder = true;
        foreach(GameObject gameObject in unitLister.enteties)
        {
            if (gameObject.GetComponent<FractionMember>().fraction == Fraction.Enemy)
            {
                OrderGiver.GiveOrderToUnits(gameObject.GetComponent<OrderableObject>(), moveOrderInitParams);
            }
        }
    }
}
