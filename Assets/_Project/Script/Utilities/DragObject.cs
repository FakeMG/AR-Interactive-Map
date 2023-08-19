using System.Collections.Generic;
using UnityEngine;

namespace FakeMG.Utilities {
    public class DragObject : MonoBehaviour {
        [SerializeField] private LayerMask ground;
        [SerializeField] private LayerMask puzzle;
        [SerializeField] private Transform vietnamModel;
        [SerializeField] private float snapDistance = 0.1f;
        [SerializeField] private float reachDistance = 5f;
    
        private Vector3 _posDiff;
        private bool _isDragging;
        private Camera _mainCamera;
        private Transform _selectedObject;
        private Dictionary<GameObject, Vector3> _originalPositions;

        private void Start() {
            _mainCamera = Camera.main;

            GetOriginalPos();
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
            
                Vector3 inputPosition = Input.mousePosition;
                if (Input.touchCount > 0) {
                    inputPosition = Input.GetTouch(0).position;
                }

                Ray ray = _mainCamera.ScreenPointToRay(inputPosition);

                if (Physics.Raycast(ray, out var hit, reachDistance, puzzle)) {
                    if (hit.collider) {
                        _isDragging = true;
                        _selectedObject = hit.transform;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) {
                _isDragging = false;
            }

            if (_isDragging) {
                DragSelectedObject();
            } else {
                SnapToPosition();
            }
        }

        private void DragSelectedObject() {
            Vector3 inputPosition = Input.mousePosition;
            if (Input.touchCount > 0) {
                inputPosition = Input.GetTouch(0).position;
            }

            Ray ray = _mainCamera.ScreenPointToRay(inputPosition);
            if (Physics.Raycast(ray, out var hit, reachDistance, ground)) {
                Vector3 pos = hit.point;
                if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
                    _posDiff = pos - _selectedObject.position;
                    _posDiff.y = 0;
                }
        
                _selectedObject.position = pos - _posDiff;
            }
        }

        private void SnapToPosition() {
            if (_selectedObject == null) return;
            if (_originalPositions == null) return;
        
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
}