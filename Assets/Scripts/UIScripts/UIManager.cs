using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using GameResources;
using HexWorldInterpretation;
using Orders;
using Orders.EntityOrder;
using PLayerScripts;
using UnitsControlScripts;
using UnityEngine;

namespace UIScripts
{
    public class UIManager : MonoBehaviour
    {
        public InputHandler inputHandler;
        public HexGrid hexGrid;
        public PlayerResoucesManager playerResoucesManager;
        
        [SerializeField] private OrderButtonsSetter orderButtonsSetter;
        [SerializeField] private ResourcesDisplayUI resourcesDisplayUI;

        void Start()
        {
            playerResoucesManager.OnUpdateResources += res => resourcesDisplayUI.UpdateResourcesValues(res);
        }
        
        public void UpdateOrderButtonsInUI(List<GameOrderInitParams> possibleOrders)
        {
            orderButtonsSetter.UpdateButtons(possibleOrders);
        }
    }
}
