using UnityEngine;

namespace FakeMG.Province {
    public class FaceCamera : MonoBehaviour {
        private Transform _cam;

        private void Awake() {
            if (Camera.main) _cam = Camera.main.transform;
        }

        private void Update() {
            if (transform.localScale != Vector3.zero) {
                Vector3 directionToCamera = _cam.position - transform.position;
                directionToCamera.y = 0f;
        
                if (directionToCamera == Vector3.zero) return;
                
                Quaternion rotation = Quaternion.LookRotation(-directionToCamera);
                transform.rotation = rotation;
            }
        }
    }
}