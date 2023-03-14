using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LookingAt : MonoBehaviour {
    [SerializeField] private float distance;
    [SerializeField] [Min(0)] private float duration;
    [SerializeField] private float localMoveDistance = 0.2f;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private LayerMask layerMask;

    private bool _currentRaycastHitted;
    private bool _previousRaycastHitted;
    private GameObject _lastObject;

    private readonly Dictionary<GameObject, Info> _originLocalPosDict = new();

    private void Update() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, layerMask.value)) {
            _currentRaycastHitted = true;
            GameObject currentObject = hit.collider.gameObject;

            if (!_originLocalPosDict.ContainsKey(currentObject)) {
                _originLocalPosDict.Add(currentObject, new Info(currentObject.transform.localPosition, 0, true));
            }
            
            if (_currentRaycastHitted && !_previousRaycastHitted) {
                if (!_originLocalPosDict[currentObject].DirectionIsUp && _originLocalPosDict[currentObject].TimePassed < duration)
                    _originLocalPosDict[currentObject].TimePassed = duration - _originLocalPosDict[currentObject].TimePassed;
            }

            if (_currentRaycastHitted && _previousRaycastHitted) {
                if (_lastObject != currentObject) {
                    if (!_originLocalPosDict[currentObject].DirectionIsUp && _originLocalPosDict[currentObject].TimePassed < duration)
                        _originLocalPosDict[currentObject].TimePassed = duration - _originLocalPosDict[currentObject].TimePassed;
                    
                    foreach (Info objectInfo in _originLocalPosDict.Values) {
                        if (objectInfo.DirectionIsUp && objectInfo.TimePassed < duration)
                            objectInfo.TimePassed = duration - objectInfo.TimePassed;
                    }
                }
            }

            //TODO: After reactivating object, it can not move up anymore
            MoveOneUp(currentObject);

            _lastObject = currentObject;
            _previousRaycastHitted = true;
        } else {
            _currentRaycastHitted = false;
            
            if (!_currentRaycastHitted && _previousRaycastHitted) {
                if (_originLocalPosDict.ContainsKey(_lastObject))
                    if (_originLocalPosDict[_lastObject].DirectionIsUp && _originLocalPosDict[_lastObject].TimePassed < duration)
                        _originLocalPosDict[_lastObject].TimePassed = duration - _originLocalPosDict[_lastObject].TimePassed;
            }
            MoveAllDown();
            
            _previousRaycastHitted = false;
            
        }
    }

    private void MoveOneUp(GameObject selectedObject) {
        foreach (GameObject currentObject in _originLocalPosDict.Keys) {
            if (currentObject != selectedObject) {
                _originLocalPosDict.TryGetValue(currentObject, out Info currentObjectInfo);
                if (currentObjectInfo == null) continue;
                
                currentObjectInfo.DirectionIsUp = false;
                var start = currentObjectInfo.LocalOrigin + localMoveDistance * Vector3.up;
                var end = currentObjectInfo.LocalOrigin;

                Move(currentObject, start, end, currentObjectInfo);
            } else {
                _originLocalPosDict.TryGetValue(selectedObject, out Info selectedObjectInfo);
                if (selectedObjectInfo == null) continue;
                
                selectedObjectInfo.DirectionIsUp = true;
                var start = selectedObjectInfo.LocalOrigin;
                var end = start + localMoveDistance * Vector3.up;

                Move(selectedObject, start, end, selectedObjectInfo);
            }
        }
    }

    private void MoveAllDown() {
        foreach (GameObject currentObject in _originLocalPosDict.Keys) {
            _originLocalPosDict.TryGetValue(currentObject, out Info currentObjectInfo);
            if (currentObjectInfo == null) continue;
            
            currentObjectInfo.DirectionIsUp = false;
            var start = currentObjectInfo.LocalOrigin + localMoveDistance * Vector3.up;
            var end = currentObjectInfo.LocalOrigin;

            Move(currentObject, start, end, currentObjectInfo);
        }
    }

    private void Move(GameObject selectedObject, Vector3 start, Vector3 end, Info objectInfo) {
        if (Vector3.Distance(selectedObject.transform.localPosition, start) == 0) {
            objectInfo.TimePassed = 0;
        }
        if (Vector3.Distance(selectedObject.transform.localPosition, end) == 0) {
            objectInfo.TimePassed = duration;
        }
        
        objectInfo.TimePassed += Time.deltaTime;
        var s = objectInfo.TimePassed / duration;

        selectedObject.transform.localPosition = Vector3.Lerp(start, end, curve.Evaluate(s));
    }

    class Info {
        public Vector3 LocalOrigin { get; set; }
        public float TimePassed { get; set; }

        public bool DirectionIsUp { get; set; }

        public Info(Vector3 localOrigin, float timePassed, bool directionIsUp) {
            LocalOrigin = localOrigin;
            TimePassed = timePassed;
            DirectionIsUp = directionIsUp;
        }
    }
}