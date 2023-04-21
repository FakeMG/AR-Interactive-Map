using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeHandler : MonoBehaviour, IDragHandler {
    [SerializeField] private float swipeSpeed = 10f;
    
    private RectTransform _infoUI;
    private float _startPosY;
    private float _targetYPos;

    private void Awake() {
        _infoUI = GetComponent<RectTransform>();

        _infoUI.anchoredPosition = new Vector2(_infoUI.anchoredPosition.x, -Screen.height + 300);
        _startPosY = -Screen.height + 300;
    }

    private void Update() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)) {
            _startPosY = _infoUI.anchoredPosition.y;
        }

        if (Input.touchCount <= 0 && !Input.GetMouseButton(0)) {
            var y = Mathf.Lerp(_infoUI.anchoredPosition.y, _targetYPos, Time.deltaTime * swipeSpeed);
            _infoUI.anchoredPosition = new Vector2(_infoUI.anchoredPosition.x, y);
        }

        if (_infoUI.anchoredPosition.y > -Screen.height / 2f) {
            _targetYPos = 0;
        } else {
            _targetYPos = -Screen.height + 300;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        float deltaY = eventData.position.y - eventData.pressPosition.y;
        var anchoredPosition = _infoUI.anchoredPosition;
        anchoredPosition = new Vector2(anchoredPosition.x, _startPosY + deltaY);
        _infoUI.anchoredPosition = anchoredPosition;
    }
}