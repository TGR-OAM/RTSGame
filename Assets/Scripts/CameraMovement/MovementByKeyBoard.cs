using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementByKeyBoard : MonoBehaviour
{
    public float speed;
  
    void Update()
    {
        MovingInputHanlder();
    }

    void MovingInputHanlder()
    {
        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            direction += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += new Vector3(-1, 0, 0);
        }
        direction.Normalize();
        TryMoveByDirection(direction);
    }

    public void TryMoveByDirection(Vector3 direction)
    {
        if (direction.magnitude != 0)
        {
            Vector3 dMove = Vector3.forward;
            Quaternion directionQ = Quaternion.LookRotation(direction, Vector3.up);
            dMove = directionQ * dMove;
            dMove = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0) * dMove;           
            dMove *= speed * Time.deltaTime;
            this.transform.position += dMove;
        }        
    }

    public void MoveToHorPositionOnMap(Vector3 position)
    {
        Vector3 newPos = this.transform.rotation * Vector3.forward;
        newPos *= this.transform.position.y/ newPos.y;

        this.transform.position = newPos + position;
    }
}
