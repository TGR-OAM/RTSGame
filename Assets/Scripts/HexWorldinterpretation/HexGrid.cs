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
        MapData = new HexGridData(512, 128, 4, 4, Default);

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
    public int width;
    public int height;

    public float cellSize;
    public float padding;

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
    }

}