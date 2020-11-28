using Assets.Scripts;
using Assets.Scripts.HexWordEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEditorScript : MonoBehaviour
{
    public HexGrid hexGrid;
    WorldEditor worldEditor;

    public float targetHeight;
    public int radiusToChange;

    // Start is called before the first frame update
    void Start()
    {
        worldEditor = new WorldEditor(hexGrid);

        XMLMapSaver.MapSaverXMLFile(hexGrid.MapData, "",SaveType.defaultSave);
    }

    void Update()
    {
        if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(Input.mousePosition), out Vector3 center))
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0))
            {
                worldEditor.TryUpdateCellHeight(HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), .5f);
            }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(1))
            {
                worldEditor.TryUpdateCellHeight(HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), -.5f);
            }

            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
            {
                worldEditor.TryUpdateCellHeightInRadius(HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), .5f, radiusToChange);
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(1))
            {
                worldEditor.TryUpdateCellHeightInRadius(HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), -.5f, radiusToChange);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(0))
            {
                worldEditor.TryUpdateCellHeight(HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), .5f, targetHeight);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1))
            {
                worldEditor.TryUpdateCellHeightInRadius(HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), .5f, radiusToChange, targetHeight);
            }
        }

    }
}
