using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class ProvinceBehaviour : MonoBehaviour {
    [SerializeField] private float speed = 6f;

    private Transform _provinceInfo;
    private Transform _landmarkInfo;

    private Vector3 _desiredSectionInfoScale;
    private Vector3 _desiredLandmarkInfoScale;
    private Vector3 _desiredPosition;
    private Vector3 _originLocalPosition;

    private void Start() {
        _originLocalPosition = transform.localPosition;
        _desiredPosition = _originLocalPosition;
        _desiredSectionInfoScale = Vector3.zero;
        _provinceInfo = transform.GetChild(0);
        _landmarkInfo = transform.GetChild(1);
    }

    private void Update() {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _desiredPosition, Time.deltaTime * speed);
        if (Vector3.Distance(transform.localPosition, _desiredPosition) <= 0.01f) {
            transform.localPosition = _desiredPosition;
        }

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

    public void RaiseProvince() {
        _desiredPosition = _originLocalPosition + Vector3.up * 0.2f;
        _desiredSectionInfoScale = Vector3.one;
        _desiredLandmarkInfoScale = Vector3.zero;
    }

    public void LowerProvince() {
        _desiredPosition = _originLocalPosition;
        _desiredSectionInfoScale = Vector3.zero;
        _desiredLandmarkInfoScale = Vector3.zero;
    }

    public void RaiseLandmark() {
        _desiredLandmarkInfoScale = Vector3.one;
        _desiredSectionInfoScale = Vector3.zero;
    }

    public bool IsProvinceUp() {
        return _desiredPosition == _originLocalPosition + Vector3.up * 0.2f;
    }

    public bool IsLandmarkUp() {
        return _desiredLandmarkInfoScale == Vector3.one;
    }
}