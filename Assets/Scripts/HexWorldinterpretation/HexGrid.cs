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
        MapData = new HexGridData(384, 128, 1, 1, Default);

        InitWorld();
    }


    void InitWorld()
    {
        #region init mesh
        hexGridRenderer = new HexGridRenderer(this);
        #endregion

        #region init coliders
        hexGridColiderer = new HexGridColiderer(this);
        #endregion
    }

    

    public bool TryRaycastHexGrid(out Vector3 output, Ray rayToCast)
    {
        return hexGridColiderer.TryRaycastHexGrid(out output, rayToCast);
    }

}

public struct HexGridData
{
    public int width { get; private set; }
    public int height { get; private set; }

    public float widthInUnits { get; private set; }
    public float heightInUnits { get; private set; }

    public float cellSize { get; private set; }
    public float padding { get; private set; }

    public float[] HeightMap;

    public Material Default;

    public HexGridData(int width,int height, float cellSize, float padding, Material Default)
    {
        this.width = width;
        this.height = height;

        this.cellSize = cellSize;
        this.padding = padding;

        this.HeightMap = new float[width* height];

        this.Default = Default;


        this.widthInUnits = width * HexMetrics.innerRadius * 2f * cellSize + (height > 1f ? HexMetrics.innerRadius * cellSize : 0);
        this.heightInUnits = 1.5f * (height - 1) * cellSize * HexMetrics.outerRadius + cellSize * HexMetrics.outerRadius * 2f;

    }

}