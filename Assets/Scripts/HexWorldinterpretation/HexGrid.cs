using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public HexGridData MapData;

    public HexGridRenderer hexGridRenderer;
    public HexGridColiderer hexGridColiderer;

    public Material Default;

    private void Awake()
    {
        MapData = new HexGridData(128, 128, 1, 1, .0001f,Default);

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


    public HexGridData(int width,int height, float cellSize, float padding, float AccurcyOfApproximation, Material Default)
    {
        this.AccurcyOfApproximation = AccurcyOfApproximation;

        this.width = width;
        this.height = height;

        this.cellSize = cellSize;
        this.padding = padding;

        this.HeightMap = new float[width* height];

        this.Default = Default;


        this.widthInUnits = width * HexMetrics.innerRadius * 2f * cellSize + (height > 1f ? HexMetrics.innerRadius * cellSize : 0);
        this.heightInUnits = 1.5f * (height - 1) * cellSize * HexMetrics.outerRadius + cellSize * HexMetrics.outerRadius * 2f;

        Buildings = new List<Building>();

    }

}