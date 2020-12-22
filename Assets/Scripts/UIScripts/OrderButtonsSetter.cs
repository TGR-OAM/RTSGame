using Orders;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class OrderButtonsSetter : MonoBehaviour
    {
        public Button ButtonPrefab;
        public UIManager uiManager;
    
        private List<Button> CurrentOrders;

        public void UpdateButtons(List<GameOrderType> possibleOrders)
        {
            if (CurrentOrders == null) CurrentOrders = new List<Button>();
        
            CurrentOrders.ForEach(x => Destroy(x.gameObject));
        
            CurrentOrders = new List<Button>();
        
            foreach (GameOrderType type in possibleOrders)
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

        public void SetOrder(GameOrderType orderType)
        {
            uiManager.inputHandler.SetOrder(orderType);
            Debug.Log(orderType);
        }
    }
}
