using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Data")]
    public PlayerData playerData;
    public TestTaskAssigner taskAssigner;
    public UIManager uiManager;

    private void Awake()
    {
        HexGrid hexGrid = GameObject.FindWithTag("HexGrid").GetComponent<HexGrid>();
        taskAssigner = GameObject.FindWithTag("Player").GetComponent<TestTaskAssigner>();
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        playerData = new PlayerData(PlayerOrderState.Idle, hexGrid);
    }

    public void SetState(PlayerOrderState state)
    {
        playerData.state = state;
    }

    public void UpdatePossibleOrders(List<Type> Orders)
    {
        //uiManager.UpdatePossibleOrdersAtInteractiveZone(Orders);
    }
}

[System.Serializable]
public struct PlayerData
{
    public PlayerOrderState state;
    public HexGrid hexGrid;

    public PlayerData(PlayerOrderState startState, HexGrid hexGrid)
    {
        state = startState;
        this.hexGrid = hexGrid;
    }

}

public enum PlayerOrderState
{ 
    Building,
    Idle,
    SettingOrder,
}

