using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitsCreator : MonoBehaviour
{
    [SerializeField]
    private float unitCreatingInterval;
    private float timeFromPreviousCreation;
    [SerializeField]
    private Transform positionToSpawnUnits;
    [SerializeField]
    private GameObject WarriorToCreate;
    [SerializeField]
    private GameObject BuilderToCreate;

    private void Start()
    {
        Instantiate(BuilderToCreate, positionToSpawnUnits.position, Quaternion.identity);
    }

    void Update()
    {
        timeFromPreviousCreation += Time.deltaTime;
        if (timeFromPreviousCreation >= unitCreatingInterval)
        {
            if (Random.value <= .2f)
            {
                Instantiate(BuilderToCreate, positionToSpawnUnits.position, Quaternion.identity);
            }
            else
            {
                Instantiate(WarriorToCreate, positionToSpawnUnits.position, Quaternion.identity);
            }
            
            timeFromPreviousCreation = 0;
        }
    }
}
