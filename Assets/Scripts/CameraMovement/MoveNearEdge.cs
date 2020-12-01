using UnityEngine;

namespace Assets.Scripts.CameraMovement
{
    public class MoveNearEdge : MonoBehaviour
    {
        public float edgeX = 150f;
        public float edgeY = 50f;

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
            GetComponent<MovementByKeyBoard>().TryMoveByDirection(direction);                      
        }
    }
}
