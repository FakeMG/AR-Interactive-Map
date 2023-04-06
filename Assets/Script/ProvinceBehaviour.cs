using UnityEngine;

public class ProvinceBehaviour : MonoBehaviour {
    [SerializeField] private float speed = 6f;
    [SerializeField] private Transform sectionInfo;

    private Vector3 _desiredInfoScale;
    private Vector3 _desiredPosition;
    private Vector3 _originLocalPosition;

    private void Start() {
        _originLocalPosition = transform.localPosition;
        _desiredPosition = _originLocalPosition;
        _desiredInfoScale = Vector3.zero;
        sectionInfo = transform.GetChild(0);
    }

    private void Update() {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _desiredPosition, Time.deltaTime * speed);
        if (Vector3.Distance(transform.localPosition, _desiredPosition) <= 0.01f) {
            transform.localPosition = _desiredPosition;
        }
        
        if (sectionInfo == null) return;
        sectionInfo.localScale = Vector3.Lerp(sectionInfo.localScale, _desiredInfoScale, Time.deltaTime * speed);
        if (Vector3.Distance(sectionInfo.localScale, _desiredInfoScale) <= 0.01f) {
            sectionInfo.localScale = _desiredInfoScale;
        }
    }

    public void RaiseProvince() {
        _desiredPosition = _originLocalPosition + Vector3.up * 0.2f;
        _desiredInfoScale = Vector3.one;
    }

    public void LowerProvince() {
        _desiredPosition = _originLocalPosition;
        _desiredInfoScale = Vector3.zero;
    }
    
    public bool IsUp() {
        return _desiredPosition == _originLocalPosition + Vector3.up * 0.2f;
    }
}