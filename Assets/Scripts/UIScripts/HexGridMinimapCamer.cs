using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridMinimapCamera
{
    public HexGrid hexGrid;
    GameObject MinimapCamera;

    public HexGridMinimapCamera(HexGrid hexGrid)
    {
        this.hexGrid = hexGrid;
        MinimapCamera = new GameObject("MinimapCamera",typeof(Camera));
        MinimapCamera.transform.parent = hexGrid.transform;
    }
}
