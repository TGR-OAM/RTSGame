using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNearEdge : MonoBehaviour
{
    float edgeX = 150f;
    float edgeY = 50f;

    void Update()
    {
        InputHandler();
    }
    void InputHandler()
    {
        Vector3 direction = new Vector3();
        if (Input.mousePosition.y > (Screen.height - edgeY))
            direction += new Vector3(0, 0, 1);

        if (Input.mousePosition.y < edgeY)
            direction += new Vector3(0, 0, -1);

        if (Input.mousePosition.x < edgeX)
            direction += new Vector3(-1, 0, 0);

        if (Input.mousePosition.x > (Screen.width - edgeX))
            direction += new Vector3(1, 0, 0);

        direction.Normalize();
        GetComponent<WSAD>().TryMoveByDirection(direction);                      
    }
}
