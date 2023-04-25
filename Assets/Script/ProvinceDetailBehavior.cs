using UnityEngine;

public class ProvinceDetailBehavior : MonoBehaviour {
    [SerializeField] private float speed = 6f;

    private readonly ProvinceInfoLoader[] _provinceInfoLoader = new ProvinceInfoLoader[2];
    private readonly Transform[] _provinceInfo = new Transform[2];
    private Transform _landmarkInfo;

    private Vector3[] _desiredSectionInfoScale = new Vector3[2];
    private Vector3 _desiredLandmarkInfoScale;

    private void Start() {
        _desiredSectionInfoScale = new Vector3[2];
        
        _provinceInfo[0] = transform.GetChild(0);
        _provinceInfo[1] = transform.GetChild(1);
        _landmarkInfo = transform.GetChild(2);
        
        _provinceInfoLoader[0] = _provinceInfo[0].GetComponent<ProvinceInfoLoader>();
        _provinceInfoLoader[1] = _provinceInfo[1].GetComponent<ProvinceInfoLoader>();
    }

    private void Update() {
        for (int i = 0; i < _provinceInfo.Length; i++) {
            if (_provinceInfo == null) continue;
            _provinceInfo[i].localScale =
                Vector3.Lerp(_provinceInfo[i].localScale, _desiredSectionInfoScale[i], Time.deltaTime * speed);
            if (Vector3.Distance(_provinceInfo[i].localScale, _desiredSectionInfoScale[i]) <= 0.01f) {
                _provinceInfo[i].localScale = _desiredSectionInfoScale[i];
            }
        }

        if (_landmarkInfo == null) return;
        _landmarkInfo.localScale =
            Vector3.Lerp(_landmarkInfo.localScale, _desiredLandmarkInfoScale, Time.deltaTime * speed);
        if (Vector3.Distance(_landmarkInfo.localScale, _desiredLandmarkInfoScale) <= 0.01f) {
            _landmarkInfo.localScale = _desiredLandmarkInfoScale;
        }
    }

    public void RaiseLandmark() {
        _desiredLandmarkInfoScale = Vector3.one;
        for (int i = 0; i < _provinceInfo.Length; i++) {
            _desiredSectionInfoScale[i] = Vector3.zero;
        }
    }

    public void RaiseProvinceInfo(string provinceName) {
        _desiredLandmarkInfoScale = Vector3.zero;

        for (int i = 0; i < _desiredSectionInfoScale.Length; i++) {
            if (_desiredSectionInfoScale[i] == Vector3.zero) {
                _desiredSectionInfoScale[i] = Vector3.one;
                _provinceInfoLoader[i].RetrieveProvinceData(provinceName);

                _desiredSectionInfoScale[(i + 1) % 2] = Vector3.zero;
                break;
            }
        }
    }

    public void LowerAll() {
        _desiredLandmarkInfoScale = Vector3.zero;
        for (int i = 0; i < _provinceInfo.Length; i++) {
            _desiredSectionInfoScale[i] = Vector3.zero;
        }
    }

    public bool IsLandmarkUp() {
        return _landmarkInfo.localScale == Vector3.one;
    }

    public void SetXAndZForClosedProvinceInfo(Vector3 position) {
        foreach (var province in _provinceInfo) {
            if (province.localScale == Vector3.zero) {
                province.position = new Vector3(position.x, transform.position.y, position.z);
            }
        }

        _landmarkInfo.position = new Vector3(position.x, transform.position.y, position.z);
    }
}