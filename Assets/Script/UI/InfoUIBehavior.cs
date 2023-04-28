using System;
using UnityEngine;

public class InfoUIBehavior : MonoBehaviour {
    [SerializeField] private float uiSwipeSpeed = 10f;
    
    private readonly RectTransform[] _infoUI = new RectTransform[2];
    
    private readonly float _notVisiblePosY = -Screen.height;
    private readonly float _lowerPosY = -Screen.height + 300;
    private const float HIGHER_POS_Y = 0f;
    private readonly float[] _targetYPos = new float[2];

    private void Awake() {
        _infoUI[0] = transform.GetChild(0).GetComponent<RectTransform>();
        _infoUI[1] = transform.GetChild(1).GetComponent<RectTransform>();

        _infoUI[0].anchoredPosition = new Vector2(_infoUI[0].anchoredPosition.x, -Screen.height);
        _infoUI[1].anchoredPosition = new Vector2(_infoUI[1].anchoredPosition.x, -Screen.height);
        
        _targetYPos[0] = _notVisiblePosY;
        _targetYPos[1] = _notVisiblePosY;
    }

    private void Update() {

        if (Input.touchCount <= 0 && !Input.GetMouseButton(0)) {
            for (int i = 0; i< _infoUI.Length; i++) {
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
            if (_infoUI[i].anchoredPosition.y > -Screen.height / 2f) {
                _targetYPos[i] = HIGHER_POS_Y;
            } else {
                _targetYPos[i] = _lowerPosY;
            }
        }
    }
    
    public void RaiseInfoUI(string provinceName) {
        for (int i = 0; i < _targetYPos.Length; i++) {
            if (Math.Abs(_targetYPos[i] - _notVisiblePosY) < 0.01f) {
                _targetYPos[i] = _lowerPosY;
                // load data

                _targetYPos[(i + 1) % 2] = _notVisiblePosY;
                break;
            }
        }
    }
    
    public void LowerAll() {
        for (int i = 0; i < _targetYPos.Length; i++) {
            _targetYPos[i] = _notVisiblePosY;
        }
    }
}