using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using HexWorldinterpretation;
using Orders;
using Orders.EntityOrder;
using UnitsControlScripts;
using UnityEngine;

namespace UIScripts
{
    public class UIManager : MonoBehaviour
    {
        public InputHandler inputHandler;
        public HexGrid hexGrid;
        [SerializeField] private OrderButtonsSetter orderButtonsSetter;
        

        public void UpdateOrderButtonsInUI(List<GameOrderInitParams> possibleOrders)
        {
            orderButtonsSetter.UpdateButtons(possibleOrders);
        }
    }
}
