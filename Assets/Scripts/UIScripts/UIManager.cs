using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public HexGrid hexGrid;
    public InteractiveZoneScript interactiveZoneScript;

    public void UpdatePossibleOrdersAtInteractiveZone(List<Type> Orders)
    {
        interactiveZoneScript.UpdatePossibleOrders(Orders);
    }
}
