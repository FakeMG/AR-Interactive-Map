using UnityEngine;

public class FaceCamera : MonoBehaviour {
    private Transform _cam;

    private void Start() {
        if (Camera.main != null) _cam = Camera.main.transform;
    }

    private void Update() {
        Vector3 directionToCamera = _cam.position - transform.position;
        directionToCamera.y = 0f;
        
        Quaternion rotation = Quaternion.LookRotation(-directionToCamera);
        transform.rotation = rotation;
    }
}