using UnityEngine;

public class ProvinceDetailBehavior : MonoBehaviour {
    [SerializeField] private float speed = 6f;
    
    private Transform _provinceInfo;
    private Transform _landmarkInfo;

    private Vector3 _desiredSectionInfoScale;
    private Vector3 _desiredLandmarkInfoScale;

    private void Start() {
        _desiredSectionInfoScale = Vector3.zero;
        _provinceInfo = transform.GetChild(0);
        _landmarkInfo = transform.GetChild(1);
    }
    
    private void Update() {
        if (_provinceInfo != null) {
            _provinceInfo.localScale =
                Vector3.Lerp(_provinceInfo.localScale, _desiredSectionInfoScale, Time.deltaTime * speed);
            if (Vector3.Distance(_provinceInfo.localScale, _desiredSectionInfoScale) <= 0.01f) {
                _provinceInfo.localScale = _desiredSectionInfoScale;
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
        _desiredSectionInfoScale = Vector3.zero;
    }
    
    public bool IsLandmarkUp() {
        return _desiredLandmarkInfoScale == Vector3.one;
    }
    
    public void RaiseProvinceInfo() {
        _desiredLandmarkInfoScale = Vector3.zero;
        _desiredSectionInfoScale = Vector3.one;
    }

    public void LowerAll() {
        _desiredLandmarkInfoScale = Vector3.zero;
        _desiredSectionInfoScale = Vector3.zero;
    }

    public void SetXAndZ(Vector3 position) {
        transform.position = new Vector3(position.x, transform.position.y, position.z);
    }
}