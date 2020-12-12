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
    private Vector3 basePosition;

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
        List<GameObject> units = unitLister.enteties;
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<FractionMember>().fraction == Fraction.Enemy && unit.GetComponent<OrderableObject>() != null)
            {
                unit.GetComponent<OrderableObject>().GiveOrder(new MoveAttackTask(basePosition, unit));
            }
        }
    }
}
