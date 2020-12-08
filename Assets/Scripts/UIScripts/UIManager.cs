using System.Collections.Generic;
using Assets.Scripts.HexWorldinterpretation;
using Assets.Scripts.UnitsControlScripts;
using UnityEngine;

namespace Assets.Scripts.UIScripts
{
    public class UIManager : MonoBehaviour
    {
        public InputHandler inputHandler;
        public HexGrid hexGrid;
        [SerializeField] private OrderButtonsSetter orderButtonsSetter;
        

        public void UpdateOrderButtonsInUI(List<OrderableObject> orderableObjects)
        {
            orderButtonsSetter.UpdateButtons(orderableObjects);
        }
    }
}
