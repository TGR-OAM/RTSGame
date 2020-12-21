using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using HexWorldinterpretation;
using UnitsControlScripts;
using UnityEngine;

namespace UIScripts
{
    public class UIManager : MonoBehaviour
    {
        public InputHandler inputHandler;
        public HexGrid hexGrid;
        [SerializeField] private OrderButtonsSetter orderButtonsSetter;
        

        public void UpdateOrderButtonsInUI(List<Type> possibleOrders)
        {
            orderButtonsSetter.UpdateButtons(possibleOrders);
        }
    }
}
