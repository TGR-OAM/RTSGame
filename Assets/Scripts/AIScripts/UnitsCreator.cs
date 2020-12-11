using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsCreator : MonoBehaviour
{
    [SerializeField]
    private float unitCreatingInterval;
    private float timeFromPreviousCreation;
    [SerializeField]
    private Vector3 creatingPos;
    [SerializeField]
    private GameObject unitToCreate;

    void Update()
    {
        timeFromPreviousCreation += Time.deltaTime;
        if (timeFromPreviousCreation >= unitCreatingInterval)
        {
            Instantiate(unitToCreate, creatingPos, Quaternion.identity);
            timeFromPreviousCreation = 0;
        }
    }
    private void OnDestroy()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
