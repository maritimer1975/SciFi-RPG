using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class SpinMe : MonoBehaviour {

        [SerializeField] float xRotationsPerMinute = 1f;
        [SerializeField] float yRotationsPerMinute = 1f;
        [SerializeField] float zRotationsPerMinute = 1f;
        
        void Update () {
            // xRotationsPerMinute X 360 / (time.deltaTime / 60)
            float xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute; // xRotationPerMinute X 360 degrees * rotation^-1 X (seconds * frame^-1 / 60 seconds * minute^-1) 
            //degrees frame^-1 = seconds frame^-1, 6-s * minutes^-1
            transform.RotateAround (transform.position, transform.right, xDegreesPerFrame);

            float yDegreesPerFrame = Time.deltaTime / 60 * 360 * yRotationsPerMinute; // 
            transform.RotateAround (transform.position, transform.up, yDegreesPerFrame);

            float zDegreesPerFrame = Time.deltaTime / 60 * 360 * zRotationsPerMinute;
            transform.RotateAround (transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}