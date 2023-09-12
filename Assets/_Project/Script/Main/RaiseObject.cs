using DG.Tweening;
using UnityEngine;

namespace FakeMG.Main {
    public class RaiseObject : MonoBehaviour {
        [SerializeField] private float duration = 1f;

        private float _localOriginalY;
        private float _localUpY;
        private float _desiredY;

        private void Start() {
            _localOriginalY = transform.localPosition.y;
            _localUpY = _localOriginalY + 1 * 0.2f;
        }

        public void RaiseProvince() {
            _desiredY = _localUpY;
            transform.DOLocalMoveY(_desiredY, duration);
        }

        public void LowerProvince() {
            _desiredY = _localOriginalY;
            transform.DOLocalMoveY(_desiredY, duration);
        }

        public bool IsProvinceUp() {
            return _desiredY == _localUpY;
        }
    }
}