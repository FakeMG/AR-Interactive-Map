using System.Collections.Generic;
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
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
                if (IsPositionOnUI(Input.mousePosition)) return;

                // Get input position
                Vector3 inputPosition = Input.mousePosition;
                if (Input.touchCount > 0) {
                    inputPosition = Input.GetTouch(0).position;
                }

                // Shoot raycast
                Ray ray = _camera.ScreenPointToRay(inputPosition);
                if (!Physics.Raycast(ray, out var hit, distance, provinceLayerMask.value)) return;

                // Get hit object
                GameObject currentObject = hit.collider.gameObject;
                if (!currentObject.TryGetComponent(out RaiseObject hitProvinceRaiser)) return;

                // If it is the same province
                if (hitProvinceRaiser.IsProvinceUp() && !provinceDetailRaiser.IsLandmarkUp()) {
                    provinceDetailRaiser.RaiseLandmark();
                    return;
                }

                if (hitProvinceRaiser.IsProvinceUp() && provinceDetailRaiser.IsLandmarkUp()) {
                    hitProvinceRaiser.LowerProvince();
                    provinceDetailRaiser.LowerAll();
                    infoUIRaiser.LowerAll();
                    return;
                }

                // If it is a different province
                //TODO: clean this mess
                hitProvinceRaiser.RaiseProvince();
                if (_preHitProvinceRaiser) {
                    _preHitProvinceRaiser.LowerProvince();
                }
                
                provinceDetailRaiser.SetPosForClosedProvinceDetail(hitProvinceRaiser.transform.position);
                provinceDetailRaiser.RaiseProvinceInfo(hitProvinceRaiser.name);
                
                infoUIDataLoader.LoadProvinceData(hitProvinceRaiser.name);
                infoUIDataLoader1.LoadProvinceData(hitProvinceRaiser.name);
                infoUIRaiser.RaiseInfoUI();
                
                _preHitProvinceRaiser = hitProvinceRaiser;
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