using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.Scripts.UIScripts;
using Assets.Scripts.UnitsControlScripts;
using UnityEngine;
using UnityEngine.UI;

public class OrderButtonsSetter : MonoBehaviour
{
    public Button ButtonPrefab;
    public UIManager uiManager;
    
    private List<Button> CurrentOrders;

    public void UpdateButtons(List<OrderableObject> orderableObjects)
    {
        if (CurrentOrders == null) CurrentOrders = new List<Button>();

        CurrentOrders.ForEach(x => Destroy(x.gameObject));
        
        CurrentOrders = new List<Button>();

        List<Type> OrderTypes = GetPossibleOrdersFromUnits(orderableObjects);
        
        foreach (Type type in OrderTypes)
        {
            Button ButtonToAdd = Instantiate(ButtonPrefab, this.transform);
            ButtonToAdd.GetComponentInChildren<Text>().text = type.ToString();
            ButtonToAdd.onClick.AddListener(delegate
            {
                SetSetOrder(type);
            });
            CurrentOrders.Add(ButtonToAdd);
        }
    }

    public List<Type> GetPossibleOrdersFromUnits(List<OrderableObject> orderableObjects)
    {
        if (orderableObjects.Count != 0 && orderableObjects != null)
        {
            List<Type> ordersType = orderableObjects[0].orderTypes;

            foreach (OrderableObject orderableObject in orderableObjects)
            {
                foreach (Type type in ordersType)
                {
                    if (!orderableObject.orderTypes.Contains(type))
                    {
                        ordersType.Remove(type);
                    }

                }
            }
            return ordersType;
        }
        else
        {
            return new List<Type>();
        }
    }

    public void SetSetOrder(Type orderType)
    {
        uiManager.inputHandler.SetOrderingState(orderType);
        //Debug.Log(orderType);
    }
}
