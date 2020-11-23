using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSAD : MonoBehaviour
{       
    float speed = 4f;
  
    void Update()
    {        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction=new Vector3(x,0,z);
        direction.Normalize();
        TryMoveByDirection(direction);
    }
    void TryMoveByDirection(Vector3 direction)
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
}
