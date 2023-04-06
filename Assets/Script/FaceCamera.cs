using UnityEngine;

public class FaceCamera : MonoBehaviour {
    private Transform _cam;

    private void Start() {
        if (Camera.main != null) _cam = Camera.main.transform;
    }

    private void Update() {
        Transform transform1;
        (transform1 = transform).LookAt(_cam);
        var temp = transform1.localEulerAngles;
        temp.x = 0;
        temp.z = 0;
        transform1.localEulerAngles = temp;
    }
}