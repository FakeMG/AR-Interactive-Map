using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FakeMG.UI {
    public class Search : MonoBehaviour {
        [SerializeField] private GameObject contentHolder;
        [SerializeField] private TMP_InputField inputField;

        private List<TextMeshProUGUI> _childrenText;
        private int _previousChildCount;

        private void Awake() {
            _childrenText = new List<TextMeshProUGUI>();

            foreach (Transform child in contentHolder.transform) {
                _childrenText.Add(child.GetChild(1).GetComponent<TextMeshProUGUI>());
            }
        }

        private void Start() {
            _previousChildCount = _childrenText.Count;
        }

        private void Update() {
            //TODO: optimize this
            if (contentHolder.transform.childCount != _previousChildCount) {
                _previousChildCount = contentHolder.transform.childCount;
                _childrenText.Clear();
                foreach (Transform child in contentHolder.transform) {
                    _childrenText.Add(child.GetChild(1).GetComponent<TextMeshProUGUI>());
                }
            }
        }

        public void SearchText() {
            foreach (var text in _childrenText) {
                int index = text.text.IndexOf(inputField.text, StringComparison.OrdinalIgnoreCase);
                bool substringExists = index != -1;

                text.transform.parent.gameObject.SetActive(substringExists);
            }
        }
    }
}