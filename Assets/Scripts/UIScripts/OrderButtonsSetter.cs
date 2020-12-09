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

    public void UpdateButtons(List<Type> possibleOrders)
    {
        if (CurrentOrders == null) CurrentOrders = new List<Button>();
        
        CurrentOrders.ForEach(x => Destroy(x.gameObject));
        
        CurrentOrders = new List<Button>();
        
        foreach (Type type in possibleOrders)
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

    public void SetOrder(Type orderType)
    {
        uiManager.inputHandler.SetOrder(orderType);
        Debug.Log(orderType);
    }
}
