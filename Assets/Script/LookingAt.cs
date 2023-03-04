using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LookingAt : MonoBehaviour {
    [SerializeField] private float distance;
    [SerializeField] [Min(0)] private float duration;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private LayerMask layerMask;

    private bool _currentRaycastHitted;
    private bool _previousRaycastHitted;
    private GameObject _lastObject;

    private readonly Dictionary<GameObject, Info> _originPos = new Dictionary<GameObject, Info>();

    private void Update() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, layerMask.value)) {
            _currentRaycastHitted = true;
            GameObject currentObject = hit.collider.gameObject;

            if (!_originPos.ContainsKey(currentObject)) {
                _originPos.Add(currentObject, new Info(currentObject.transform.position, 0, true));
            }
            
            if (_currentRaycastHitted && !_previousRaycastHitted) {
                if (!_originPos[currentObject].DirectionIsUp && _originPos[currentObject].TimePassed < duration)
                    _originPos[currentObject].TimePassed = duration - _originPos[currentObject].TimePassed;
            }

            if (_currentRaycastHitted && _previousRaycastHitted) {
                if (_lastObject != currentObject) {
                    if (!_originPos[currentObject].DirectionIsUp && _originPos[currentObject].TimePassed < duration)
                        _originPos[currentObject].TimePassed = duration - _originPos[currentObject].TimePassed;
                    
                    foreach (Info objectInfo in _originPos.Values) {
                        if (objectInfo.DirectionIsUp && objectInfo.TimePassed < duration)
                            objectInfo.TimePassed = duration - objectInfo.TimePassed;
                    }
                }
            }

            MoveOneUp(currentObject);

            _lastObject = currentObject;
            _previousRaycastHitted = true;
        } else {
            _currentRaycastHitted = false;
            
            if (!_currentRaycastHitted && _previousRaycastHitted) {
                if (_originPos.ContainsKey(_lastObject))
                    if (_originPos[_lastObject].DirectionIsUp && _originPos[_lastObject].TimePassed < duration)
                        _originPos[_lastObject].TimePassed = duration - _originPos[_lastObject].TimePassed;
            }
            MoveAllDown();
            
            _previousRaycastHitted = false;
            
        }
    }

    private void MoveOneUp(GameObject selectedObject) {
        foreach (GameObject currentObject in _originPos.Keys) {
            if (currentObject != selectedObject) {
                _originPos.TryGetValue(currentObject, out Info currentObjectInfo);
                if (currentObjectInfo == null) continue;
                
                currentObjectInfo.DirectionIsUp = false;
                var start = currentObjectInfo.Origin + Vector3.up;
                var end = currentObjectInfo.Origin;

                Move(currentObject, start, end, currentObjectInfo);
            } else {
                _originPos.TryGetValue(selectedObject, out Info selectedObjectInfo);
                if (selectedObjectInfo == null) continue;
                
                selectedObjectInfo.DirectionIsUp = true;
                var start = selectedObjectInfo.Origin;
                var end = start + Vector3.up;

                Move(selectedObject, start, end, selectedObjectInfo);
            }
        }
    }

    private void MoveAllDown() {
        foreach (GameObject currentObject in _originPos.Keys) {
            _originPos.TryGetValue(currentObject, out Info currentObjectInfo);
            if (currentObjectInfo == null) continue;
            
            currentObjectInfo.DirectionIsUp = false;
            var start = currentObjectInfo.Origin + Vector3.up;
            var end = currentObjectInfo.Origin;

            Move(currentObject, start, end, currentObjectInfo);
        }
    }

    private void Move(GameObject selectedObject, Vector3 start, Vector3 end, Info objectInfo) {
        if (Vector3.Distance(selectedObject.transform.position, start) == 0) {
            objectInfo.TimePassed = 0;
        }
        if (Vector3.Distance(selectedObject.transform.position, end) == 0) {
            objectInfo.TimePassed = duration;
        }
        
        objectInfo.TimePassed += Time.deltaTime;
        var s = objectInfo.TimePassed / duration;

        selectedObject.transform.position = Vector3.Lerp(start, end, curve.Evaluate(s));
    }

    class Info {
        public Vector3 Origin { get; set; }
        public float TimePassed { get; set; }

        public bool DirectionIsUp { get; set; }

        public Info(Vector3 origin, float timePassed, bool directionIsUp) {
            Origin = origin;
            TimePassed = timePassed;
            DirectionIsUp = directionIsUp;
        }
    }
}