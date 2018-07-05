using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraController : MonoBehaviour {

        [SerializeField] Transform target;
        Vector3 offset;
        
        [Tooltip("Elevation of the camera [Degrees]")] 
        [SerializeField] float elevation = 45f;

        [Tooltip("Distance to the target [Units]")]
        [SerializeField] float distanceToTarget = 10f;
        
        [Tooltip("Rotation on the ground plane [Degrees]")]
        [SerializeField] float azimuth = 45f;

        void Start()
        {
            // calculate the x, y and z position of the camera based on elevation, distance to target and rotation
            // x = 
            // y = 
            // 

            // float hypotenuse = distanceToTarget * Mathf.Cos(elevation);  // temporary adjacent side of the elevation triangle, which is the hypotenuse of the azmiuth triangle
            
            // float x = hypotenuse * Mathf.Sin(azimuth * Mathf.PI / 180); //
            // float y = distanceToTarget * Mathf.Sin(elevation * Mathf.PI / 180); //
            // float z = hypotenuse * Mathf.Cos(azimuth * Mathf.PI / 180);
            // offset = new Vector3(x, y, z);
        }


        private void LateUpdate()
        {
            float hypotenuse = distanceToTarget * Mathf.Cos(elevation * Mathf.PI / 180);  // temporary adjacent side of the elevation triangle, which is the hypotenuse of the azmiuth triangle
            
            float x = hypotenuse * Mathf.Sin(azimuth * Mathf.PI / 180); //
            float y = distanceToTarget * Mathf.Sin(elevation * Mathf.PI / 180); //
            float z = hypotenuse * Mathf.Cos(azimuth * Mathf.PI / 180);
            offset = new Vector3(x, y, z);           
            
            
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}