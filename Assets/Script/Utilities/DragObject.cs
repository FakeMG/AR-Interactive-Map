using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DragObject : MonoBehaviour {
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform cube;
    [SerializeField] private float speed;

    private float _diff = 0;
    private float _OGMouseY = 0;
    private float _OGCubeZ;
    private Vector3 _posDiff;
    private bool _isDragging;
    private Camera _mainCamera;
    private Vector3 _previousMouseWorldPosition;
    
    public Camera arCamera;
    public ARRaycastManager arRaycastManager;

    private void Start() {
        _mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hit)) {
                if (hit.collider != null) {
                    _isDragging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            _isDragging = false;
        }

        if (_isDragging) {
            var cubePosition = cube.position;
            Vector3 mousePosition = Input.mousePosition;

            var screenCenter = arCamera.ViewportToScreenPoint(mousePosition);
            var hits = new List<ARRaycastHit>();
            arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
            
            if (hits.Count > 0) {
                Vector3 pos = hits[0].pose.position;
                if (Input.GetMouseButtonDown(0)) {
                    _posDiff = pos - cubePosition;
                    _posDiff.y = 0;
                }
                cube.position = pos - _posDiff;
            }
        }
    }
}