using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LookingAt : MonoBehaviour {
    [SerializeField] private float distance;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float rayCastInterval = 30f;
    private float _timer;
    private readonly List<ProvinceBehaviour> _provinceBehaviours = new();

    private void Start() {
        ProvinceBehaviour[] provinceBehaviours = FindObjectsOfType<ProvinceBehaviour>();
        _provinceBehaviours.AddRange(provinceBehaviours);
    }

    private void Update() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, layerMask.value)) {
            GameObject currentObject = hit.collider.gameObject;
            _timer = 0;
            
            if (Input.GetMouseButtonDown(0)) {
                if (currentObject.TryGetComponent(out ProvinceBehaviour provinceBehaviour)) {
                    if (provinceBehaviour.IsProvinceUp() && !provinceBehaviour.IsLandmarkUp()) {
                        provinceBehaviour.RaiseLandmark();
                        return;
                    }
                    
                    if (provinceBehaviour.IsProvinceUp() && provinceBehaviour.IsLandmarkUp()) {
                        provinceBehaviour.LowerProvince();
                        return;
                    }
                    
                    provinceBehaviour.RaiseProvince();
                    foreach (ProvinceBehaviour province in _provinceBehaviours) {
                        if (province != provinceBehaviour) {
                            province.LowerProvince();
                        }
                    }
                }
            }
            
        } else {
            _timer += Time.deltaTime;
        }
        
        if (_timer >= rayCastInterval) {
            foreach (ProvinceBehaviour province in _provinceBehaviours) {
                province.LowerProvince();
            }
            _timer = 0;
        }
    }
}