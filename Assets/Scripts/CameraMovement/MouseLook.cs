using UnityEngine;

namespace CameraMovement
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity;

        private void Start()
        {
            CameraRotation();
        }
        void Update()
        {        
            if (Input.GetMouseButton(2))
            {
                CameraRotation();
            }
        }
        void CameraRotation()
        {
            float mouseY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseX = Mathf.Clamp(transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * mouseSensitivity, 60, 80);
            this.transform.localEulerAngles = new Vector3(mouseX, mouseY, 0f);
        }
    }
}