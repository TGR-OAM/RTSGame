using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRayCast : MonoBehaviour
{
    public HexGrid hexGrid;
    WorldEditor worldEditor;

    private void Awake()
    {
        worldEditor = new WorldEditor(hexGrid);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0))
        {
            if (hexGrid.TryRaycastHexGrid(out Vector3 output, Camera.main.ScreenPointToRay(Input.mousePosition))) 
            {
                worldEditor.TryUpdateCellHeight(output, .5f);
            }
                
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(1))
        {
            if (hexGrid.TryRaycastHexGrid(out Vector3 output, Camera.main.ScreenPointToRay(Input.mousePosition))) 
            {
                worldEditor.TryUpdateCellHeight(output, -.5f);
            }

        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(0))
        {
            if (hexGrid.TryRaycastHexGrid(out Vector3 output, Camera.main.ScreenPointToRay(Input.mousePosition)))
            {
                worldEditor.TryUpdateCellHeightInRadius(output, .5f, 5);
            }
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1))
        {
            if (hexGrid.TryRaycastHexGrid(out Vector3 output, Camera.main.ScreenPointToRay(Input.mousePosition)))
            {
                worldEditor.TryUpdateCellHeightInRadius(output, -.5f, 5);
            }
        }
    }
}
