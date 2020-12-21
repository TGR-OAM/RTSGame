﻿using HexWorldinterpretation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraMovement
{
    public class MovementByMinimap : MonoBehaviour
    {
        public HexGrid hexGrid;

        public void ClickedOnMiniMap()
        {
            Vector2 mousePositon = Mouse.current.position.ReadValue();
            float relXCursor = (mousePositon.x - (this.transform.position.x - this.GetComponent<RectTransform>().rect.width/2))/ this.GetComponent<RectTransform>().rect.width * hexGrid.MapData.widthInUnits;
            float relYCursor = (mousePositon.y - (this.transform.position.y - this.GetComponent<RectTransform>().rect.height / 2))/ this.GetComponent<RectTransform>().rect.height * hexGrid.MapData.heightInUnits;

            Vector3 newPosition = new Vector3(relXCursor, 0, relYCursor);

            Camera.main.transform.GetComponent<MovementByKeyBoard>().MoveToHorPositionOnMap(newPosition);
        }
    }
}
