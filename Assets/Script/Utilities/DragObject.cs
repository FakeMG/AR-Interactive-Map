using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DragObject : MonoBehaviour {
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask puzzle;
    [SerializeField] private Transform vietnamModel;
    [SerializeField] private float snapDistance = 0.1f;
    [SerializeField] private float reachDistance = 5f;
    [SerializeField] private Camera arCamera;
    [SerializeField] private ARRaycastManager arRaycastManager;

    private Vector3 _posDiff;
    private bool _isDragging;
    private Camera _mainCamera;
    private Transform _selectedObject;
    private Dictionary<GameObject,Vector3> _originalPositions;

    private void Start() {
        _mainCamera = Camera.main;
        
        //TODO: remove this later
        GetOriginalPos();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hit, reachDistance, puzzle)) {
                if (hit.collider != null) {
                    _isDragging = true;
                    _selectedObject = hit.transform;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            _isDragging = false;
        }

        if (_isDragging) {
            DragSelectedObject();
        } else {
            SnapToPosition();
        }
    }

    private void DragSelectedObject() {
        // var cubePosition = _selectedObject.position;
        // Vector3 mousePosition = Input.mousePosition;
        //
        // var screenCenter = arCamera.ViewportToScreenPoint(mousePosition);
        // var hits = new List<ARRaycastHit>();
        // arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        //
        // if (hits.Count > 0) {
        //     Vector3 pos = hits[0].pose.position;
        //     if (Input.GetMouseButtonDown(0)) {
        //         _posDiff = pos - cubePosition;
        //         _posDiff.y = 0;
        //     }
        //
        //     _selectedObject.position = pos - _posDiff;
        // }

        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out var hit, reachDistance, ground)) {
            Vector3 pos = hit.point;
            if (Input.GetMouseButtonDown(0)) {
                _posDiff = pos - _selectedObject.position;
                _posDiff.y = 0;
            }

            _selectedObject.position = pos - _posDiff;
        }
    }

    private void SnapToPosition() {
        if (_selectedObject == null) return;
        
        foreach (var pos in _originalPositions) {
            if (Vector3.Distance(_selectedObject.position, pos.Value) < snapDistance) {
                _selectedObject.position = pos.Value;
            }
        }
    }

    public void GetOriginalPos() {
        _originalPositions = new Dictionary<GameObject, Vector3>();
        foreach (Transform child in vietnamModel) {
            _originalPositions.Add(child.gameObject, child.position);
        }
    }
}