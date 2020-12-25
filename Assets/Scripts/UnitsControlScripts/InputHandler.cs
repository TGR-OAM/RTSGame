using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Buildings;
using CameraMovement;
using HexWorldinterpretation;
using Orders;
using Orders.EntityOrder;
using UIScripts;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace UnitsControlScripts
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

        [Header("Unit control properties")] 
        [SerializeField] private List<OrderableObject> SelectedEnteties = new List<OrderableObject>();
        public List<GameOrderInitParams> PossibleOrders = new List<GameOrderInitParams>();

        private Fraction fraction = Fraction.Player;
        private bool isSelecting = false;

        [Header("Building")] [Description("If state is building")]
        private Builder builder;

        [SerializeField] private bool isBuilding = false;
        //[SerializeField] private Building TestPrefab;

        [Header("Input property")]
        [SerializeField] private PlayerInput playerInput;

        [Header("Ordering")]
        [SerializeField] private GameOrderInitParams CurrentOrder;
        
        [Header("General references")]
        [SerializeField] private OrderGiver orderGiver;

        [SerializeField] private MovementByKeyBoard camera;         

        private Vector2 mousePosition;

        private void Start()
        {
            orderGiver = new OrderGiver(hexGrid,this);
            builder = new Builder(hexGrid);

            
            InputActionMap PlayerActionMap = playerInput.actions.FindActionMap("Player");
            
            PlayerActionMap.FindAction("SelectUnit").started += _ => SelectEnteties();
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

        public void SetStateFromOrderType(GameOrderInitParams orderType)
        {
            switch(orderType)
            {
                case BuildOrderInitParams buildOrderInitParams:
                    currentState = HandlerState.Building;
                    builder.StartPlacingBuilding((orderType as BuildOrderInitParams).building);
                    break;
                case UnitCreationOrderInitParams unitCreationOrderInitParams:
                    foreach (Building building in SelectedEnteties.Where(x => x.TryGetComponent(typeof(Building), out _)).Select(x => x.GetComponent<Building>()))
                    {
                        UnitCreationOrder unitCreationOrder = unitCreationOrderInitParams.CreateOrder(new UnitCreationOrderVariableParams(building.CreationOutput, building.gameObject)) as UnitCreationOrder;
                        building.orderableObject.GiveOrder(unitCreationOrder);
                    }
                    break;
                default:
                    CurrentOrder = orderType;
                    currentState = HandlerState.Ordering;
                    break;
            }
            
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
                    orderGiver.GiveOrderWithNonFixedParams(SelectedEnteties.ToArray(), new MoveAttackOrderInitParams());
                }
                else
                {
                    orderGiver.GiveOrderWithNonFixedParams(SelectedEnteties.ToArray(), new GameOrderInitParams(), true);
                }
            }
            else if (currentState == HandlerState.Ordering)
            {
                orderGiver.GiveOrderWithNonFixedParams(SelectedEnteties.ToArray(), CurrentOrder);
                ReturnToIdleState();
            }
            else if (currentState == HandlerState.Building)
            {
                if (builder.CanPlaceFlyingBuilding())
                {
                    BuildOrderVariableParams buildOrderVariableParams =
                        new BuildOrderVariableParams(builder.flyingBuilding, SelectedEnteties[0].gameObject);
                    BuildOrder buildOrder = new BuildOrder(buildOrderVariableParams);
                    
                    SelectedEnteties[0].GiveOrder(buildOrder);
                    
                    builder.PlaceFlyingBuilding();
                    
                    ReturnToIdleState();
                }
            }
        }

        public void SelectEnteties()
        {
            if (!EventSystem.current.IsPointerOverGameObject() && currentState == HandlerState.Idle && !isSelecting)
            {
                selectionBox.gameObject.SetActive(true);
                isSelecting = true;
                startPos = mousePosition;
                UpdateSelectionBox(mousePosition);
            }
        }
        
        public void ReleaseSelectionBox()
        {
            if (currentState == HandlerState.Idle && isSelecting)
            {
                Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
                Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
                SelectedEnteties = new List<OrderableObject>();
                selectionBox.gameObject.SetActive(false);

                if (selectionBox.sizeDelta.magnitude <= 10f)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hitBuilding, 100f, 1<<8 | 1 << 10))
                    {
                        if (hitBuilding.transform.gameObject.GetComponent<FractionMember>().fraction == fraction)
                        {
                            SelectedEnteties.Add(hitBuilding.transform.GetComponent<OrderableObject>());
                        }
                    }
                }
                else
                {
                    List<GameObject> AllUnits = EntitiesLister.enteties;
                    foreach (GameObject Entety in AllUnits)
                    {
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(Entety.transform.position);

                        if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y &&
                            Entety.GetComponent<FractionMember>().fraction == fraction &&
                            Entety.TryGetComponent(typeof(Unit), out _))
                        {
                            SelectedEnteties.Add(Entety.GetComponent<OrderableObject>());
                        }

                        
                    }
                }

                isSelecting = false;
                selectionBox.sizeDelta = new Vector2();
                
                PossibleOrders = GetPossibleOrdersFromUnits(SelectedEnteties.ToList());
                uiManager.UpdateOrderButtonsInUI(PossibleOrders);
                
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
        
        private List<GameOrderInitParams> GetPossibleOrdersFromUnits(List<OrderableObject> orderableObjects)
        {
            if (orderableObjects.Count != 0 && orderableObjects != null)
            {
                List<GameOrderInitParams> ordersType = new List<GameOrderInitParams>();
                ordersType.AddRange(orderableObjects[0].GameOrderInitParamsArray);
                foreach (OrderableObject orderableObject in orderableObjects)
                {
                    ordersType =
                        new List<GameOrderInitParams>(
                            orderableObject.GameOrderInitParamsArray.Where(x => ordersType
                                .Select(y => x.GetType() == y.GetType()).Any())
                            );
                }
                return ordersType;
            }
            else
            {
                return new List<GameOrderInitParams>();
            }
        }
    }
}
