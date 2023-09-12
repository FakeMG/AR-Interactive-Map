using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FakeMG.Main.UI {
    public class SwipeHandler : MonoBehaviour, IDragHandler {
        [SerializeField] private CanvasScaler canvasScaler;
    
        private RectTransform _infoUI;
        private float _startPosY;
        private float _targetYPos;
        private float _canvasMultiplier;

        private void Awake() {
            _infoUI = GetComponent<RectTransform>();
            _canvasMultiplier = Screen.width / canvasScaler.referenceResolution.x;

            _startPosY = _infoUI.anchoredPosition.y;
        }

        private void Update() {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)) {
                _startPosY = _infoUI.anchoredPosition.y;
            }
        }

        public void OnDrag(PointerEventData eventData) {
            float deltaY = eventData.position.y - eventData.pressPosition.y;
            deltaY /= _canvasMultiplier;
        
            if (_startPosY + deltaY >= 0) deltaY = -_startPosY;
        
            var anchoredPosition = _infoUI.anchoredPosition;
            anchoredPosition = new Vector2(anchoredPosition.x, _startPosY + deltaY);
            _infoUI.anchoredPosition = anchoredPosition;
        }
    }
}