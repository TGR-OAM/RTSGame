using Orders;
using System;
using System.Collections.Generic;
using Orders.EntityOrder;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class OrderButtonsSetter : MonoBehaviour
    {
        public Button ButtonPrefab;
        public UIManager uiManager;
    
        private List<Button> CurrentOrders;

        public void UpdateButtons(List<GameOrderInitParams> possibleOrders)
        {
            if (CurrentOrders == null) CurrentOrders = new List<Button>();
        
            CurrentOrders.ForEach(x => Destroy(x.gameObject));
        
            CurrentOrders = new List<Button>();
        
            foreach (GameOrderInitParams type in possibleOrders)
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

        public void SetOrder(GameOrderInitParams orderType)
        {
            uiManager.inputHandler.SetStateFromOrderType(orderType);
            Debug.Log(orderType);
        }
    }
}
