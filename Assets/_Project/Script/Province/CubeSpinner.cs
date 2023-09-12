using UnityEngine;

namespace FakeMG.Province {
    public class CubeSpinner : MonoBehaviour {
        public float spinSpeed = 50f; // Rotation speed in degrees per second

        // Update is called once per frame
        void Update() {
            if (transform.localScale == Vector3.zero) return;
        
            // Get the current rotation of the cube
            Vector3 currentRotation = transform.rotation.eulerAngles;

            // Calculate the new rotation based on the spin speed and time elapsed
            float newRotationY = currentRotation.y + (spinSpeed * Time.deltaTime);

            // Apply the new rotation to the cube's transform
            transform.rotation = Quaternion.Euler(currentRotation.x, newRotationY, currentRotation.z);
        }
    }
}