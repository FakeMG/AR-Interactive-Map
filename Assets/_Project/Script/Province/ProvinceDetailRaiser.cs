using System.Collections.Generic;
using FakeMG.Utilities;
using UnityEngine;

namespace FakeMG.Province {
    public class ProvinceDetailRaiser : MonoBehaviour {
        private readonly ProvinceInfoLoader[] _provinceInfoLoader = new ProvinceInfoLoader[2];
        private readonly ScaleObject[] _provinceInfo = new ScaleObject[2];
        
        private List<ScaleObject> _landmarkInfoList;
        private int _currentLandmarkIndex = -1;

        private void Awake() {
            _provinceInfo[0] = transform.GetChild(0).GetComponent<ScaleObject>();
            _provinceInfo[1] = transform.GetChild(1).GetComponent<ScaleObject>();

            _provinceInfoLoader[0] = _provinceInfo[0].GetComponent<ProvinceInfoLoader>();
            _provinceInfoLoader[1] = _provinceInfo[1].GetComponent<ProvinceInfoLoader>();
        }

        public void RaiseNextLandmark() {
            foreach (var provinceInfo in _provinceInfo) {
                provinceInfo.ScaleDown();
            }

            if (_currentLandmarkIndex != -1) {
                _landmarkInfoList[_currentLandmarkIndex].ScaleDown();
            }

            _currentLandmarkIndex++;
            _landmarkInfoList[_currentLandmarkIndex].ScaleUp();
        }

        public void RaiseProvinceInfo(string provinceName) {
            if (_landmarkInfoList != null) {
                foreach (var landmarkInfo in _landmarkInfoList) {
                    landmarkInfo.ScaleDown();
                }
            }
            
            for (int i = 0; i < _provinceInfo.Length; i++) {
                if (_provinceInfo[i].IsUp()) continue;

                _provinceInfo[i].ScaleUp();
                _provinceInfo[(i + 1) % 2].ScaleDown();
                _provinceInfoLoader[i].RetrieveProvinceData(provinceName);
                break;
            }
        }

        public void LowerAll() {
            if (_landmarkInfoList != null) {
                foreach (var landmarkInfo in _landmarkInfoList) {
                    landmarkInfo.ScaleDown();
                }
            }
            
            foreach (var provinceInfo in _provinceInfo) {
                provinceInfo.ScaleDown();
            }
            
            _currentLandmarkIndex = -1;
        }

        public bool IsLastLandmarkUp() {
            if (_landmarkInfoList == null || _landmarkInfoList.Count == 0) return true;
            return _landmarkInfoList[^1].IsUp();
        }

        public string GetCurrentLandmarkName() {
            return _landmarkInfoList[_currentLandmarkIndex].name;
        }
        
        public void SetLandmarkList(List<ScaleObject> landmarkInfoList) {
            _landmarkInfoList = landmarkInfoList;
        }

        public void SetPosForClosedProvinceDetail(Vector3 position) {
            foreach (var province in _provinceInfo) {
                if (!province.IsUp()) {
                    province.transform.position = new Vector3(position.x, position.y + 0.7f, position.z);
                }
            }
        }
    }
}