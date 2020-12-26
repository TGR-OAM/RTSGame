using System.Collections;
using System.Collections.Generic;
using Orders.EntityOrder;
using Units;
using UnitsControlScripts;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject boom;

    public Robot robot;
    public float damage { get; private set; }= 10;
    public float hp { get; private set; } = 30;
    public float attackdistance { get; private set; } = 20;

    void Start() 
    {
        robot.weaponInit();
    }
    void Update()
    {
        
    }

    public void Shoot(WhoAttakingDelegate whoAttakingDelegate)
    {
        Debug.Log(gameObject);
        Instantiate(boom, transform.GetChild(0).transform);
    }
}
