using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace FakeMG.Main.UI {
    public class InfoUIRaiser : MonoBehaviour {
        [SerializeField] private float uiSwipeSpeed = 10f;
        [FormerlySerializedAs("topSection")] [SerializeField] private RectTransform bottomButtonGroup;
        [SerializeField] private RectTransform canvas;

        private float _canvasHeight;

        private readonly InfoUIDataLoader[] _infoUIDataLoader = new InfoUIDataLoader[2];
        private readonly RectTransform[] _infoUI = new RectTransform[2];

        private float _notVisiblePosY;
        private float _lowerPosY;
        private const float HIGHER_POS_Y = 0f;
        private readonly float[] _targetYPos = new float[2];

        private void Awake() {

            StartCoroutine(WaitUntilEndOfFrame());

            _infoUI[0] = transform.GetChild(0).GetComponent<RectTransform>();
            _infoUI[1] = transform.GetChild(1).GetComponent<RectTransform>();
            
            _infoUIDataLoader[0] = _infoUI[0].GetComponent<InfoUIDataLoader>();
            _infoUIDataLoader[1] = _infoUI[1].GetComponent<InfoUIDataLoader>();
        }

        // The layout group script (as well as content size fitters) need a frame to update before you can get the correct element size values
        IEnumerator WaitUntilEndOfFrame() {
            yield return new WaitForEndOfFrame();
            _canvasHeight = canvas.rect.height;
            float handlerHeight = 20;
            _lowerPosY = -(_canvasHeight - bottomButtonGroup.rect.height - handlerHeight);
            _notVisiblePosY = -_canvasHeight;

            _infoUI[0].anchoredPosition = new Vector2(_infoUI[0].anchoredPosition.x, _notVisiblePosY);
            _infoUI[1].anchoredPosition = new Vector2(_infoUI[1].anchoredPosition.x, _notVisiblePosY);

            _targetYPos[0] = _notVisiblePosY;
            _targetYPos[1] = _notVisiblePosY;
        }

        private void Update() {
            if (Input.touchCount <= 0 && !Input.GetMouseButton(0)) {
                for (int i = 0; i < _infoUI.Length; i++) {
                    var y = Mathf.Lerp(_infoUI[i].anchoredPosition.y, _targetYPos[i], Time.deltaTime * uiSwipeSpeed);
                    _infoUI[i].anchoredPosition = new Vector2(_infoUI[i].anchoredPosition.x, y);

                    if (Mathf.Abs(_infoUI[i].anchoredPosition.y - _targetYPos[i]) <= 0.01f) {
                        var anchoredPosition = _infoUI[i].anchoredPosition;
                        anchoredPosition = new Vector2(anchoredPosition.x, _targetYPos[i]);
                        _infoUI[i].anchoredPosition = anchoredPosition;
                    }
                }
            }

            for (int i = 0; i < _infoUI.Length; i++) {
                if (Math.Abs(_targetYPos[i] - _notVisiblePosY) < 0.01f) continue;
                if (_infoUI[i].anchoredPosition.y > -_canvasHeight / 2f) {
                    _targetYPos[i] = HIGHER_POS_Y;
                } else {
                    _targetYPos[i] = _lowerPosY;
                }
            }
        }

        public void RaiseInfoUI(string path, string dataName) {
            for (int i = 0; i < _targetYPos.Length; i++) {
                if (Math.Abs(_targetYPos[i] - _notVisiblePosY) < 0.01f) {
                    _targetYPos[i] = _lowerPosY;

                    _targetYPos[(i + 1) % 2] = _notVisiblePosY;
                    _infoUIDataLoader[i].LoadDataToUI(path, dataName);
                    break;
                }
            }
        }
        
        public void RaiseInfoUI(TextMeshProUGUI peopleName) {
            RaiseInfoUI("people", peopleName.text);
        }

        public void LowerAll() {
            for (int i = 0; i < _targetYPos.Length; i++) {
                _targetYPos[i] = _notVisiblePosY;
            }
        }
    }
}