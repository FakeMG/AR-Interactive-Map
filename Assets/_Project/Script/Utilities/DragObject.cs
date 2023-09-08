using FakeMG.ScriptableObject;
using UnityEngine;

namespace FakeMG.Utilities {
    public class DragObject : MonoBehaviour {
        [SerializeField] private LayerMask ground;
        [SerializeField] private LayerMask puzzle;
        [SerializeField] private float snapDistance = 0.1f;
        [SerializeField] private float reachDistance = 5f;
        [SerializeField] private OriginalPosition originalPosition;
    
        private Vector3 _posDiff;
        private bool _isDragging;
        private Camera _mainCamera;
        private Transform _selectedObject;

        private void Start() {
            _mainCamera = Camera.main;
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
            if (!_selectedObject) return;
            if (!originalPosition) return;
        
            foreach (var pos in originalPosition.OriginalPositions) {
                if (Vector3.Distance(_selectedObject.position, pos.Value) < snapDistance) {
                    _selectedObject.position = pos.Value;
                }
            }
        }
    }
}