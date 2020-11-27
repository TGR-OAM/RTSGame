﻿using Assets.Scripts;
using Assets.Scripts.HexWorldinterpretation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public string DefMapPath;

    public HexGridData MapData;

    public HexGridRenderer hexGridRenderer { get; private set; }
    public HexGridColiderer hexGridColiderer { get; private set; }

    private void Awake()
    {
        MapData = XMLMapParser.LoadXMLFileMap(DefMapPath)[0];

        InitWorld();
    }


    void InitWorld()
    {
        #region init mesh
        hexGridRenderer = new HexGridRenderer(this);
        #endregion

        #region init coliders
        hexGridColiderer = new HexGridColiderer(this,8);
        #endregion
    }

    

    public bool TryRaycastHexGrid( Ray rayToCast, out Vector3 output)
    {
        return hexGridColiderer.TryRaycastHexGrid( rayToCast, out output);
    }

}

[System.Serializable]
public struct HexGridData
{
    #region this gets with resource load

    public string name { get; private set; }
    public float AccurcyOfApproximation { get; private set; }
     
    public int width { get; private set; }
    public int height { get; private set; }

    public float widthInUnits { get; private set; }
    public float heightInUnits { get; private set; }

    public float cellSize { get; private set; }
    public float padding { get; private set; }

    public float[] HeightMap;

    #endregion

    #region this isnt permanent solution
    public Material Default;
    #endregion

    #region other information about map
    public List<Building> Buildings;
    #endregion


    public HexGridData(string name,int width,int height, float cellSize, float padding, float AccurcyOfApproximation, Material Default)
    {
        this.name = name;

        this.width = width;
        this.height = height;

        this.cellSize = cellSize;
        this.padding = padding;

        this.HeightMap = new float[width* height];

        this.HeightMap[0] = 5;

        this.Default = Default;

        this.AccurcyOfApproximation = AccurcyOfApproximation;

        this.widthInUnits = width * HexMetrics.innerRadius * 2f * cellSize + (height > 1f ? HexMetrics.innerRadius * cellSize : 0);
        this.heightInUnits = 1.5f * (height - 1) * cellSize * HexMetrics.outerRadius + cellSize * HexMetrics.outerRadius * 2f;

        Buildings = new List<Building>();

    }
    public HexGridData(string name, int width, int height, float cellSize, float padding, float[] HeightMap,float AccurcyOfApproximation, Material Default)
    {
        this.name = name;


        this.width = width;
        this.height = height;

        this.cellSize = cellSize;
        this.padding = padding;

        this.HeightMap = HeightMap;

        this.Default = Default;

        this.AccurcyOfApproximation = AccurcyOfApproximation;

        this.widthInUnits = width * HexMetrics.innerRadius * 2f * cellSize + (height > 1f ? HexMetrics.innerRadius * cellSize : 0);
        this.heightInUnits = 1.5f * (height - 1) * cellSize * HexMetrics.outerRadius + cellSize * HexMetrics.outerRadius * 2f;

        Buildings = new List<Building>();

    }

}