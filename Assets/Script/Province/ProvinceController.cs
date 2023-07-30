using System.Collections.Generic;
using FakeMG.UI;
using FakeMG.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace FakeMG.Province {
    public class ProvinceController : MonoBehaviour {
        [SerializeField] private float distance;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float rayCastInterval = 30f;
        [FormerlySerializedAs("provinceDetailBehavior")] [SerializeField] private ProvinceDetailRaiser provinceDetailRaiser;
        [FormerlySerializedAs("infoUIBehavior")] [SerializeField] private InfoUIRaiser infoUIRaiser;
        [SerializeField] private InfoUIDataLoader infoUIDataLoader;
        [SerializeField] private InfoUIDataLoader infoUIDataLoader1;

        private readonly List<RaiseObject> _provinceRaisers = new();

        private float _timer;
        private Camera _camera;

        private void Start() {
            _camera = Camera.main;

            RaiseObject[] provinceBehaviours = FindObjectsOfType<RaiseObject>();
            _provinceRaisers.AddRange(provinceBehaviours);
        }

        private void Update() {
            // LowerProvincesAfterSomeTime();

            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
                if (IsPositionOnUI(Input.mousePosition)) return;

                Vector3 inputPosition = Input.mousePosition;
                if (Input.touchCount > 0) {
                    inputPosition = Input.GetTouch(0).position;
                }

                Ray ray = _camera.ScreenPointToRay(inputPosition);
                if (!Physics.Raycast(ray, out var hit, distance, layerMask.value)) return;

                GameObject currentObject = hit.collider.gameObject;
                if (!currentObject.TryGetComponent(out RaiseObject provinceRaiser)) return;

                if (provinceRaiser.IsProvinceUp() && !provinceDetailRaiser.IsLandmarkUp()) {
                    provinceDetailRaiser.RaiseLandmark();
                    return;
                }

                if (provinceRaiser.IsProvinceUp() && provinceDetailRaiser.IsLandmarkUp()) {
                    provinceRaiser.LowerProvince();
                    provinceDetailRaiser.LowerAll();
                    infoUIRaiser.LowerAll();
                    return;
                }

                //TODO: clean this mess
                provinceRaiser.RaiseProvince();
                provinceDetailRaiser.SetPosForClosedProvinceDetail(provinceRaiser.transform.position);
                provinceDetailRaiser.RaiseProvinceInfo(provinceRaiser.name);
                infoUIDataLoader.LoadProvinceData(provinceRaiser.name);
                infoUIDataLoader1.LoadProvinceData(provinceRaiser.name);
                infoUIRaiser.RaiseInfoUI();

                foreach (RaiseObject province in _provinceRaisers) {
                    if (province != provinceRaiser) {
                        province.LowerProvince();
                    }
                }
            }
        }

        private void LowerProvincesAfterSomeTime() {
            if (Physics.Raycast(transform.position, transform.forward, distance, layerMask.value)) {
                _timer = 0;
            } else {
                _timer += Time.deltaTime;
            }

            if (_timer >= rayCastInterval) {
                foreach (RaiseObject province in _provinceRaisers) {
                    province.LowerProvince();
                    provinceDetailRaiser.LowerAll();
                    infoUIRaiser.LowerAll();
                }

                _timer = 0;
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
}