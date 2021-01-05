using System;
using CameraMovement;
using HexWorldinterpretation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HexWordEditor
{
    public class TestEditorScript : MonoBehaviour
    {
        public HexGrid hexGrid;
        WorldEditor worldEditor;

        public float targetHeight;
        public int radiusToChange;
        public int deltaHeight;

        public InputField inputField;

        public Color curColor;

        public MovementByKeyBoard movementScript;
        public PlayerInput PlayerInput;
        private InputActionMap editorMap;

        // Start is called before the first frame update
        void Start()
        {
            worldEditor = new WorldEditor(hexGrid);

            inputField.text = hexGrid.MapData.name;

            editorMap = PlayerInput.actions.FindActionMap("EditNewMaps");

            editorMap.FindAction("ChangeHeight").performed += ctx => ChangeHeightByValue(ctx.ReadValue<float>());
        }

        private void Update()
        {
            movementScript.TryMoveByDirection( editorMap.FindAction("MoveCamera").ReadValue<Vector2>());
        }

        public void ChangeHeightByValue(float value)
        {

            if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out Vector3 center))
            {
                if (editorMap.FindAction("Area(no target height)").ReadValue<float>()>.1f)
                {
                    worldEditor.TryUpdateCellHeightInRadius(
                        HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), deltaHeight*value, radiusToChange);
                }
                else if (editorMap.FindAction("Area").ReadValue<float>()>.1f)
                {
                    worldEditor.TryUpdateCellHeightInRadius(HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), deltaHeight, radiusToChange, targetHeight);
                }
                else if(editorMap.FindAction("PaintFlag").ReadValue<float>()>.1f)
                {
                    worldEditor.TryUpdateCellColor(HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), curColor);
                }
                else
                {
                    worldEditor.TryUpdateCellHeight(
                        HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), deltaHeight * value);
                }
            }
        }
        
        
        /*void Update()
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

                if(Input.GetKey(KeyCode.P) && Input.GetMouseButton(0))
                {
                    worldEditor.TryUpdateCellColor(HexMetrics.CalcHexCoordXZFromDefault(center, hexGrid.MapData.cellSize), curColor);
                }
            }
        }
        */

        public void SaveMap()
        {
            if(inputField.text != "")
                XMLMapSaver.MapSaverXMLFile(hexGrid.MapData, "SavedMaps/"+ inputField.text, SaveType.overrideSave);
        }

        public void OnColorButtonClick(string color)
        {
            switch (color)
            {
                case "yellow":
                    curColor = Color.yellow;
                    break;
                case "blue":
                    curColor = Color.blue;
                    break;
                case "green":
                    curColor = Color.green;
                    break;
            }
        }
    }
}
