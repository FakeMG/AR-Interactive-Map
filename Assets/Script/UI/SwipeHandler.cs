using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeHandler : MonoBehaviour, IDragHandler {
    private RectTransform _infoUI;
    private float _startPosY;
    private float _targetYPos;

    private void Awake() {
        _infoUI = GetComponent<RectTransform>();

        _startPosY = _infoUI.anchoredPosition.y;
    }

    private void Update() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)) {
            _startPosY = _infoUI.anchoredPosition.y;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        float deltaY = eventData.position.y - eventData.pressPosition.y;
        var anchoredPosition = _infoUI.anchoredPosition;
        anchoredPosition = new Vector2(anchoredPosition.x, _startPosY + deltaY);
        _infoUI.anchoredPosition = anchoredPosition;
    }
}