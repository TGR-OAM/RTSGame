using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementByMinimap : MonoBehaviour
{
    public HexGrid hexGrid;

   public void ClickedOnMiniMap()
    {
        float relXCursor = (Input.mousePosition.x - (this.transform.position.x - this.GetComponent<RectTransform>().rect.width/2))/ this.GetComponent<RectTransform>().rect.width * hexGrid.MapData.widthInUnits;
        float relYCursor = (Input.mousePosition.y - (this.transform.position.y - this.GetComponent<RectTransform>().rect.height / 2))/ this.GetComponent<RectTransform>().rect.height * hexGrid.MapData.heightInUnits;

        Debug.Log(relXCursor + " " + relYCursor);

        Vector3 newPosition = new Vector3(relXCursor, 0, relYCursor);

        Camera.main.transform.GetComponent<MovementByKeyBoard>().MoveToHorPositionOnMap(newPosition);
    }
}
