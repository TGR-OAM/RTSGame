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
    private Vector3 creatingPos;
    [SerializeField]
    private GameObject WarriorToCreate;
    [SerializeField]
    private GameObject BuilderToCreate;

    private void Start()
    {
        //Instantiate(BuilderToCreate, creatingPos, Quaternion.identity);
    }

    void Update()
    {
        timeFromPreviousCreation += Time.deltaTime;
        if (timeFromPreviousCreation >= unitCreatingInterval)
        {

            Instantiate(WarriorToCreate, creatingPos, Quaternion.identity);

            timeFromPreviousCreation = 0;
        }
    }
}
