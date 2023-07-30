using UnityEngine;

namespace FakeMG.Utilities {
    public class RaiseObject : MonoBehaviour {
        [SerializeField] private float speed = 6f;

        private Vector3 _desiredPosition;
        private Vector3 _originLocalPosition;
        private Vector3 _upPosition;

        private void Start() {
            _originLocalPosition = transform.localPosition;
            _desiredPosition = _originLocalPosition;
            _upPosition = _originLocalPosition + Vector3.up * 0.2f;
        }

        private void Update() {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _desiredPosition, Time.deltaTime * speed);
            if (Vector3.Distance(transform.localPosition, _desiredPosition) <= 0.0001f) {
                transform.localPosition = _desiredPosition;
            }
        }

        public void RaiseProvince() {
            _desiredPosition = _upPosition;
        }

        public void LowerProvince() {
            _desiredPosition = _originLocalPosition;
        }

        public bool IsProvinceUp() {
            return _desiredPosition == _upPosition;
        }
    }
}