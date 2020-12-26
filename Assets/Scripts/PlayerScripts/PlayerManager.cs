using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Buildings;
using CameraMovement;
using HexWorldinterpretation;
using PLayerScripts;
using UIScripts;
using UnitsControlScripts;
using UnityEngine;

[RequireComponent(typeof(InputHandler), typeof(PlayerResoucesManager))]
public class PlayerManager : MonoBehaviour
{
    public OrderGiver OrderGiver { get; private set; }
    public Builder Builder { get; private set; }
    
    [Header("Movement description")]
    [SerializeField] public MovementByKeyBoard camera;

    [Header("World description")]
    [SerializeField] public HexGrid hexGrid;

    [Header("UI description")]
    [SerializeField] public UIManager uiManager;
    
    [Header("Input description")]
    [SerializeField] public InputHandler InputHandler;
    
    [Header("Player description")]
    [SerializeField] public Fraction fraction = Fraction.Player;

    [Header("Resources description")] [SerializeField]
    public PlayerResoucesManager PlayerResoucesManager;

    private void Start()
    {
        OrderGiver = new OrderGiver(hexGrid,InputHandler);
        Builder = new Builder(hexGrid, this);
    }
}
