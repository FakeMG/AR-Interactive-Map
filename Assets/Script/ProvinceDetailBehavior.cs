using UnityEngine;

public class ProvinceDetailBehavior : MonoBehaviour {
    private readonly ProvinceInfoLoader[] _provinceInfoLoader = new ProvinceInfoLoader[2];
    private readonly ScaleObject[] _provinceInfo = new ScaleObject[2];
    private ScaleObject _landmarkInfo;

    private void Start() {
        _provinceInfo[0] = transform.GetChild(0).GetComponent<ScaleObject>();
        _provinceInfo[1] = transform.GetChild(1).GetComponent<ScaleObject>();
        _landmarkInfo = transform.GetChild(2).GetComponent<ScaleObject>();
        
        _provinceInfoLoader[0] = _provinceInfo[0].GetComponent<ProvinceInfoLoader>();
        _provinceInfoLoader[1] = _provinceInfo[1].GetComponent<ProvinceInfoLoader>();
    }

    public void RaiseLandmark() {
        foreach (var provinceInfo in _provinceInfo) {
            provinceInfo.ScaleDown();
        }

        _landmarkInfo.ScaleUp();
    }

    public void RaiseProvinceInfo(string provinceName) {
        _landmarkInfo.ScaleDown();
        
        for (int i = 0; i < _provinceInfo.Length; i++) {
            if (_provinceInfo[i].IsUp()) continue;
            
            _provinceInfo[i].ScaleUp();
            _provinceInfo[(i + 1) % 2].ScaleDown();
            _provinceInfoLoader[i].RetrieveProvinceData(provinceName);
            break;
        }
    }

    public void LowerAll() {
        _landmarkInfo.ScaleDown();
        foreach (var provinceInfo in _provinceInfo) {
            provinceInfo.ScaleDown();
        }
    }

    public bool IsLandmarkUp() {
        return _landmarkInfo.IsUp();
    }

    public void SetPosForClosedProvinceDetail(Vector3 position) {
        foreach (var province in _provinceInfo) {
            if (!province.IsUp()) {
                province.transform.position = new Vector3(position.x, transform.position.y, position.z);
            }
        }

        _landmarkInfo.transform.position = new Vector3(position.x, transform.position.y, position.z);
    }
}