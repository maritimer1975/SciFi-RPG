using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraController : MonoBehaviour {

        [SerializeField] Transform target;
        Vector3 offset;
        
        [Tooltip("Look at the player or free look camera?")]
        [SerializeField] bool isCameraTopDown = true;

        [Header("Top Down Camera Controls")]

        [Tooltip("Elevation of the camera [Degrees]")] 
        [SerializeField] float elevation = 45f;

        [Tooltip("Distance to the target [Units]")]
        [SerializeField] float distanceToTarget = 10f;
        
        [Tooltip("Rotation on the ground plane [Degrees]")]
        [SerializeField] float azimuth = 45f;

        [Header("3rd Person Camera Controls")]

        [SerializeField] Vector3 cameraOffset = new Vector3(0,0,0);

        [Tooltip("Rotation on the vertical [Degrees]")]
        [SerializeField] float verticalRotation = 45f;

        [Tooltip("Rotation on the horixontal [Degrees]")]
        [SerializeField] float horizontalRotation = 45f;

        [SerializeField] float speedYaw = 0f;
        [SerializeField] float speedPitch = 0f;

        private float yaw = 0f;
        private float pitch = 0f;


        Camera mainCamera;

        void Start()
        {
            mainCamera = GetComponentInChildren<Camera>();

            DefaultCameraRotPos();
        }

        void Update()
        {
            //if(!isCameraTopDown)
            //{
                // control the camera rotation
                yaw = speedYaw * Input.GetAxis("Mouse X");
                pitch = speedPitch * Input.GetAxis ("Mouse Y");

                float localEulerAngleY = transform.localEulerAngles.y;

                //transform.localEulerAngles = new Vector3(pitch, yaw, 0f);
                if( localEulerAngleY > 90f ) 
                {
                    yaw = 0f;
                }
                
                transform.RotateAround(target.transform.position, Vector3.up, yaw);

                //transform.RotateAround(target.transform.position, Vector3.forward, pitch);

                Debug.Log("localEulerAngles = " + localEulerAngleY);
                // Debug.Log("Yaw = " + yaw);

            //}
        }

        private void DefaultCameraRotPos()
        {
            if (isCameraTopDown)
            {
                float hypotenuse = distanceToTarget * Mathf.Cos(elevation * Mathf.PI / 180);  // temporary adjacent side of the elevation triangle, which is the hypotenuse of the azmiuth triangle

                float x = hypotenuse * Mathf.Sin(azimuth * Mathf.PI / 180); //
                float y = distanceToTarget * Mathf.Sin(elevation * Mathf.PI / 180); //
                float z = hypotenuse * Mathf.Cos(azimuth * Mathf.PI / 180);
                offset = new Vector3(x, y, z);

                transform.position = target.position + offset;
                transform.LookAt(target);
            }
            else
            {
                //Vector3 lookAt = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane);
                transform.localPosition = cameraOffset;
                transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
                //transform.LookAt(lookAt, Vector3.up);
            }
        }
    }
}