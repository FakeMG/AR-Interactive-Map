using System.Collections.Generic;
using FakeMG.Database;
using FakeMG.UI;
using FakeMG.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FakeMG.Province {
    public class TouchProvinceHandler : MonoBehaviour {
        [SerializeField] private float distance;
        [SerializeField] private LayerMask provinceLayerMask;
        [SerializeField] private ProvinceDetailRaiser provinceDetailRaiser;
        [SerializeField] private InfoUIRaiser infoUIRaiser;
        [SerializeField] private InfoUIDataLoader infoUIDataLoader;
        [SerializeField] private InfoUIDataLoader infoUIDataLoader1;

        private RaiseObject _preHitProvinceRaiser;
        private Camera _camera;

        private void Awake() {
            _camera = Camera.main;
        }

        private void Update() {
            //TODO: clean this mess
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
                // Get input position
                Vector3 inputPosition = GetInputPosition();
                if (IsPositionOnUI(inputPosition)) return;

                // Shoot raycast
                Ray ray = _camera.ScreenPointToRay(inputPosition);
                if (!Physics.Raycast(ray, out var hit, distance, provinceLayerMask.value)) return;

                // Get hit object
                GameObject currentObject = hit.collider.gameObject;
                if (!currentObject.TryGetComponent(out RaiseObject hitProvinceRaiser)) return;

                // If it is the same province and the last landmark is NOT up
                if (hitProvinceRaiser.IsProvinceUp() && !provinceDetailRaiser.IsLastLandmarkUp()) {
                    provinceDetailRaiser.RaiseNextLandmark();
                    return;
                }

                // If it is the same province and the last landmark is up
                if (hitProvinceRaiser.IsProvinceUp() && provinceDetailRaiser.IsLastLandmarkUp()) {
                    hitProvinceRaiser.LowerProvince();
                    provinceDetailRaiser.LowerAll();
                    infoUIRaiser.LowerAll();
                    return;
                }

                // If it is a different province
                // Lower previous province
                if (_preHitProvinceRaiser) {
                    _preHitProvinceRaiser.LowerProvince();
                }

                hitProvinceRaiser.RaiseProvince();
                _preHitProvinceRaiser = hitProvinceRaiser;

                // Raise province detail and info UI
                provinceDetailRaiser.SetPosForClosedProvinceDetail(hitProvinceRaiser.transform.position);
                provinceDetailRaiser.RaiseProvinceInfo(hitProvinceRaiser.name);

                // Get new landmark info list
                List<ScaleObject> children = new List<ScaleObject>();
                foreach (Transform child in currentObject.transform) {
                    children.Add(child.gameObject.GetComponent<ScaleObject>());
                }

                provinceDetailRaiser.SetLandmarkInfoList(children);

                infoUIDataLoader.LoadDataToUI("provinces", hitProvinceRaiser.name);
                infoUIDataLoader1.LoadDataToUI("provinces", hitProvinceRaiser.name);
                infoUIRaiser.RaiseInfoUI();
            }
        }

        private Vector3 GetInputPosition() {
            if (Input.touchCount > 0) {
                return Input.GetTouch(0).position;
            }

            return Input.mousePosition;
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