using UnityEngine;

namespace Assets.Scripts.CameraMovement
{
    public class MovementByKeyBoard : MonoBehaviour
    {
        public float speed;
  

        public void TryMoveByDirection(Vector2 direction)
        {
            if (direction.magnitude != 0)
            {
                Vector3 directionV3 = new Vector3(direction.x, 0, direction.y);
                Vector3 dMove = Vector3.forward;
                Quaternion directionQ = Quaternion.LookRotation(directionV3, Vector3.up);
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
}
