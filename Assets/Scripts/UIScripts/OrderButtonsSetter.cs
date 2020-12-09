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
                SetOrder(type);
            });
            CurrentOrders.Add(ButtonToAdd);
        }
    }

    public List<Type> GetPossibleOrdersFromUnits(List<OrderableObject> orderableObjects)
    {
        if (orderableObjects.Count != 0 && orderableObjects != null)
        {
            List<Type> ordersType = new List<Type>();
            ordersType.AddRange(orderableObjects[0].orderTypes);
            foreach (OrderableObject orderableObject in orderableObjects)
            {
                for (int i = 0; i < ordersType.Count; i++)
                {
                    if (!orderableObject.orderTypes.Contains(ordersType[i]))
                    {
                        ordersType.RemoveAt(i);
                        i--;
                    }

                }
                Debug.Log(ordersType.Count);
            }
            return ordersType;
        }
        else
        {
            return new List<Type>();
        }
    }

    public void SetOrder(Type orderType)
    {
        uiManager.inputHandler.SetOrder(orderType);
        Debug.Log(orderType);
    }
}
