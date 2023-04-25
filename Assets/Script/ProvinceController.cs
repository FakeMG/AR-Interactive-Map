using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProvinceController : MonoBehaviour {
    [SerializeField] private float distance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayCastInterval = 30f;
    [SerializeField] private ProvinceDetailBehavior provinceDetailBehavior;

    private readonly List<ProvinceBehaviour> _provinceBehaviours = new();

    private float _timer;
    private Camera _camera;

    private void Start() {
        _camera = Camera.main;

        ProvinceBehaviour[] provinceBehaviours = FindObjectsOfType<ProvinceBehaviour>();
        _provinceBehaviours.AddRange(provinceBehaviours);
    }

    private void Update() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, layerMask.value)) {
            _timer = 0;
        } else {
            _timer += Time.deltaTime;
        }

        if (_timer >= rayCastInterval) {
            foreach (ProvinceBehaviour province in _provinceBehaviours) {
                province.LowerProvince();
            }

            _timer = 0;
        }

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
            if (IsPositionOnUI(Input.mousePosition)) return;

            Vector3 inputPosition = Input.mousePosition;
            if (Input.touchCount > 0) {
                inputPosition = Input.GetTouch(0).position;
            }

            Ray ray = _camera.ScreenPointToRay(inputPosition);
            if (!Physics.Raycast(ray, out hit, distance, layerMask.value)) return;

            GameObject currentObject = hit.collider.gameObject;
            if (!currentObject.TryGetComponent(out ProvinceBehaviour provinceBehaviour)) return;

            if (provinceBehaviour.IsProvinceUp() && !provinceDetailBehavior.IsLandmarkUp()) {
                provinceDetailBehavior.RaiseLandmark();
                return;
            }

            if (provinceBehaviour.IsProvinceUp() && provinceDetailBehavior.IsLandmarkUp()) {
                provinceBehaviour.LowerProvince();
                provinceDetailBehavior.LowerAll();
                return;
            }

            provinceBehaviour.RaiseProvince();
            provinceDetailBehavior.SetXAndZForClosedProvinceInfo(provinceBehaviour.transform.position);
            provinceDetailBehavior.RaiseProvinceInfo(provinceBehaviour.name);

            foreach (ProvinceBehaviour province in _provinceBehaviours) {
                if (province != provinceBehaviour) {
                    province.LowerProvince();
                }
            }
        }
    }

    private bool IsPositionOnUI(Vector3 position) {
        PointerEventData eventData = new PointerEventData(EventSystem.current) {
            position = position
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}