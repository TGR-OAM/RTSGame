using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts.Buildings;
using Assets.Scripts.CameraMovement;
using Assets.Scripts.HexWorldinterpretation;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.UIScripts;
using Assets.Scripts.Units;
using Assets.Scripts.UnitsControlScripts.UnitsControlScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.Scripts.UnitsControlScripts
{
    public class InputHandler : MonoBehaviour
    {
        private enum HandlerState
        {
            Idle,
            Building,
            Ordering
        }

        [SerializeField] private HandlerState currentState = HandlerState.Idle;
        [SerializeField] private HexGrid hexGrid;

        [Header("UI properties")] [SerializeField]
        private RectTransform selectionBox;

        [SerializeField] private UIManager uiManager;

        private Vector2 startPos;

        [Header("Unit control properties")] [SerializeField]
        private List<Unit> units = new List<Unit>();

        private Fraction fraction = Fraction.Player;
        [SerializeField] private UnitLister lister;
        private bool isSelecting = false;

        [Header("Building")] [Description("If state is building")]
        private Builder builder;

        [SerializeField] private bool isBuilding = false;
        [SerializeField] private Building TestPrefab;

        [Header("Input property")]
        [SerializeField] private PlayerInput playerInput;

        [Header("Ordering")]
        [SerializeField] private Type CurrentOrder;
        
        [Header("General references")]
        [SerializeField] private OrderGiver orderGiver;

        [SerializeField] private MovementByKeyBoard camera;         

        private Vector2 mousePosition;

        private void Start()
        {
            orderGiver = new OrderGiver(hexGrid);
            builder = new Builder(hexGrid);

            
            InputActionMap PlayerActionMap = playerInput.actions.FindActionMap("Player");
            
            PlayerActionMap.FindAction("SelectUnit").started += _ => SelectUnits();
            PlayerActionMap.FindAction("SelectUnit").canceled += _ => ReleaseSelectionBox();
            PlayerActionMap.FindAction("GiveOrderToUnit").performed += _ => GiveOrderToUnits();
            PlayerActionMap.FindAction("SetIdleState").performed += _ => ReturnToIdleState();
            
        }

        private void Update()
        {
            CameraMovement(playerInput.actions.FindActionMap("Player").FindAction("MoveCamera").ReadValue<Vector2>());

            mousePosition = Mouse.current.position.ReadValue();
            switch (currentState)

            {
                case HandlerState.Idle:
                    if (isSelecting)
                    {
                        UpdateSelectionBox(mousePosition);
                    }
                    else if (selectionBox.gameObject.activeSelf)
                    {
                        selectionBox.gameObject.SetActive(false);
                    }

                    break;
                case HandlerState.Building:
                    OperateBuildingPlacing();

                    break;
            }
        }

        public void SetOrder(Type orderType)
        {
            if (orderType == typeof(BuildTask))
            {
                currentState = HandlerState.Building;
                builder.StartPlacingBuilding(TestPrefab);
            }
            else
            {
                CurrentOrder = orderType;
                currentState = HandlerState.Ordering;
            }
            
        }
        
        public void SetBuildingState(Building building)
        {
            currentState = HandlerState.Building;
        }
        
        private void OperateBuildingPlacing()
        {
            builder.Update();
        }

        public void ReturnToIdleState()
        {
            builder.StopPlacingBuilding();
            currentState = HandlerState.Idle;
        }

        public void GiveOrderToUnits()
        {
            if (currentState == HandlerState.Idle)
            {
                if (playerInput.actions.FindActionMap("Player").FindAction("APressed").ReadValue<float>() >= .5f)
                {
                    orderGiver.GiveOrder(units.ToArray(), typeof(MoveAttackTask));
                }
                else
                {
                    orderGiver.GiveOrder(units.ToArray(), null, true);
                }
            }
            else if (currentState == HandlerState.Ordering)
            {
                orderGiver.GiveOrder(units.ToArray(), CurrentOrder);
                ReturnToIdleState();
            }
            else if (currentState == HandlerState.Building)
            {
                BuildTask buildTask = new BuildTask(builder.flyingBuilding,units[0].gameObject);
                if (builder.TryPlaceFlyingBuilding())
                {
                    units[0].orderableObject.GiveOrder(buildTask);
                    ReturnToIdleState();
                }
                
            }
        }

        public void SelectUnits()
        {
            if (!EventSystem.current.IsPointerOverGameObject() && currentState == HandlerState.Idle && !isSelecting)
            {
                selectionBox.gameObject.SetActive(true);
                isSelecting = true;
                startPos = mousePosition;
            }
        }

        public void ReleaseSelectionBox()
        {
            if (currentState == HandlerState.Idle && isSelecting)
            {
                Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
                Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
                units.Clear();
                selectionBox.gameObject.SetActive(false);

                List<GameObject> AllUnits = lister.units;
                foreach (GameObject unit in AllUnits)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

                    if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y &&
                        unit.GetComponent<FractionMember>().fraction == fraction)
                    {
                        units.Add(unit.GetComponent<Unit>());
                    }

                    isSelecting = false;
                }
                uiManager.UpdateOrderButtonsInUI(units.Select(x => x.orderableObject).ToList());
                
            }
        }

        private void UpdateSelectionBox(Vector2 curMousePos)
        {
            float width = curMousePos.x - startPos.x;
            float height = curMousePos.y - startPos.y;

            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
        }

        public void ChangeState(int newState)
        {
            currentState = (HandlerState) newState;
        }

        public void Build(Building building)
        {
            if (currentState == HandlerState.Building && !EventSystem.current.IsPointerOverGameObject())
                builder.StartPlacingBuilding(building);
        }
        public void CameraMovement(Vector2 direction)
        {
            camera.TryMoveByDirection(direction);       
        }
    }
}
