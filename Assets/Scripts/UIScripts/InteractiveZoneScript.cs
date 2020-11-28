using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveZoneScript : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public Transform ButtonsPart;

    List<GameObject> OrderButtons = new List<GameObject>();
    public void UpdatePossibleOrders(List<Type> Orders)
    {
        foreach (GameObject OrderButton in OrderButtons)
        {
            GameObject.Destroy(OrderButton);
        }
        OrderButtons = new List<GameObject>();

        
    }
}
