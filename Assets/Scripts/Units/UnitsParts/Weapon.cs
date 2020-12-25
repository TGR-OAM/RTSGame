using System.Collections;
using System.Collections.Generic;
using Orders.EntityOrder;
using UnitsControlScripts;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject boom;
    
    void Start()
    {

    }
    void Update()
    {
        
    }

    public void Shoot(WhoAttakingDelegate whoAttakingDelegate)
    {
        Instantiate(boom, transform.GetChild(0).transform);
    }
}
