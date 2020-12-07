using System.Collections.Generic;
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
        [SerializeField]
        private HandlerState currentState = HandlerState.Idle;
        [SerializeField]
        private HexGrid hexGrid;

        [Header("UI properties")]
        [SerializeField]
        private RectTransform selectionBox;
        private Vector2 startPos;

    [Header("Unit control properties")]
    [SerializeField]
    private List<Unit> units = new List<Unit>();
    private Fraction fraction = Fraction.Player;
    [SerializeField]
    private UnitLister lister;
    private bool isSelecting = false;

    [Header("Building properties")]
    private Builder builder;
    private bool isBuilding = false;

    [Header("Input property")]
    [SerializeField]
    private PlayerInput playerInput;
    
    [SerializeField]
    private OrderGiver orderGiver;


    private Vector2 mousePosition;

    private void Start()
    {
        orderGiver = new OrderGiver(hexGrid);
        builder = new Builder(hexGrid);

        playerInput.actions.FindActionMap("Player").FindAction("SelectUnit").started += _ => SelectUnits();
        playerInput.actions.FindActionMap("Player").FindAction("SelectUnit").canceled += _ => ReleaseSelectionBox();
    }

    private void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();

        switch (currentState)
        {
            case HandlerState.Idle:
                if (isSelecting)
                {
                    UpdateSelectionBox(mousePosition);
                }
                else if (selectionBox.gameObject.activeInHierarchy)
                {
                    selectionBox.gameObject.SetActive(false);
                }
                break;
            case HandlerState.Building:
                if (isBuilding)
                {
                    OperateBuildingPlacing();
                }
                break;
        }
    }

    private void OperateBuildingPlacing()
    {
        builder.Update();
    }

    private void ReturnToIdleState()
    {
        builder.StopPlacingBuilding();
        currentState = HandlerState.Idle;
    }

    public void GiveOrderToUnits()
    {
        orderGiver.GiveOrder(units.ToArray(), OrderType.None);
    }

    public void SelectUnits()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && currentState == HandlerState.Idle && !isSelecting)
        {
            selectionBox.gameObject.SetActive(true);
            isSelecting = true;
            startPos = mousePosition;
            print("huy" + Time.time);
        }

    public void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
        units.Clear();
        List<GameObject> fullUnitList = lister.units;
        foreach (GameObject unit in fullUnitList)
        {
            selectionBox.gameObject.SetActive(false);

            Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
            Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
            units.Clear();
            List<GameObject> guys = lister.units;
            foreach (GameObject unit in guys)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y && unit.GetComponent<FractionMember>().fraction == fraction)
                {
                    units.Add(unit.GetComponent<Unit>());
                }
            }
        }
        print("not huy");
        isSelecting = false;
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
        currentState = (HandlerState)newState;
    }

    public void Build(Building building)
    {
        if (currentState == HandlerState.Building && !EventSystem.current.IsPointerOverGameObject()) builder.StartPlacingBuilding(building);
    }
}
