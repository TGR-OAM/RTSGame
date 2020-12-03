using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

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
    private List<Unit> units = new List<Unit>();
    private Fraction fraction = Fraction.Player;
    [SerializeField]
    private UnitLister lister;

    [Header("Building properties")]
    private Builder builder;

    [SerializeField]
    private OrderGiver orderGiver;
    //[SerializeField]
    //private string unitListerTag;

    private void Start()
    {
        orderGiver = new OrderGiver(hexGrid);
        builder = new Builder(hexGrid);
    }

    private void Update()
    {
        switch (currentState)
        {
            case HandlerState.Idle:
                SelectUnits();
                GiveOrderToUnits();
                break;
            case HandlerState.Building:
                OperateBuildingPlacing();
                OperateBuildingPlacing();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToIdleState();
        }
    }

    private void OperateBuildingPlacing()
    {
        builder.isMouseButtonPressed = Input.GetMouseButtonDown(0);
        builder.Update();
    }

    private void ReturnToIdleState()
    {
        builder.StopPlacingBuilding();
        currentState = HandlerState.Idle;
    }

    private void GiveOrderToUnits()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Input.GetKey(KeyCode.Q))
            {
                orderGiver.GiveOrder(units.ToArray(), OrderType.MoveAttack);
            } else
            {
                orderGiver.GiveOrder(units.ToArray(), OrderType.None);
            }
        }
    }

    private void SelectUnits()
    {
        if (!EventSystem.current.IsPointerOverGameObject() || selectionBox.gameObject.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                UpdateSelectionBox(Input.mousePosition);
            }
            if (Input.GetMouseButtonUp(0))
            {
                ReleaseSelectionBox();
            }
        }
    }

    private void ReleaseSelectionBox()
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

    private void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    public void ChangeState(int newState)
    {
        switch (newState)
        {
            case 0:
                currentState = HandlerState.Idle;
                break;
            case 1:
                currentState = HandlerState.Building;
                break;
            case 2:
                currentState = HandlerState.Ordering;
                break;
        }
    }

    public void Build(Building building)
    {
        if (currentState == HandlerState.Building) builder.StartPlacingBuilding(building);
    }
}
