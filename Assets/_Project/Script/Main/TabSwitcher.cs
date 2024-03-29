﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FakeMG.Main {
    public class TabSwitcher : MonoBehaviour {
        [SerializeField] private GameObject buttonGroup;

        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;
        [SerializeField] private List<GameObject> provinceObjects;
        [SerializeField] private List<GameObject> peopleObjects;
        [SerializeField] private List<GameObject> puzzleObjects;

        [SerializeField] private UnityEvent onSceneSwitch;
        [SerializeField] private UnityEvent onProvinceSwitch;
        [SerializeField] private UnityEvent onPeopleSwitch;
        [SerializeField] private UnityEvent onPuzzleSwitch;

        private List<List<GameObject>> _correspondingObjects;
        private readonly List<GameObject> _buttons = new();

        private void Awake() {
            foreach (Transform child in buttonGroup.transform) {
                _buttons.Add(child.gameObject);
            }

            _correspondingObjects = new List<List<GameObject>> {
                provinceObjects,
                peopleObjects,
                puzzleObjects
            };
            
            ActivateButton(_buttons[0]);
        }

        public void ActivateButton(GameObject button) {
            ResetAllButtons();
            HighlightButton(button);
            ToggleCorrespondingElements(button, true);
            onSceneSwitch?.Invoke();
            
            if (button == _buttons[0]) {
                onProvinceSwitch?.Invoke();
            } else if (button == _buttons[1]) {
                onPeopleSwitch?.Invoke();
            } else if (button == _buttons[2]) {
                onPuzzleSwitch?.Invoke();
            }
        }

        private void ResetAllButtons() {
            foreach (GameObject button in _buttons) {
                UnhighlightButton(button);
                ToggleCorrespondingElements(button, false);
            }
        }

        private void UnhighlightButton(GameObject button) {
            var buttonRect = button.GetComponent<RectTransform>();
            var buttonImage = button.GetComponent<UnityEngine.UI.Image>();
            
            buttonRect.sizeDelta = new Vector2(75, 75);
            buttonImage.color = inactiveColor;
        }

        private void HighlightButton(GameObject button) {
            var buttonRect = button.GetComponent<RectTransform>();
            var buttonImage = button.GetComponent<UnityEngine.UI.Image>();
            
            buttonRect.sizeDelta = new Vector2(100, 100);
            buttonImage.color = activeColor;
        }

        private void ToggleCorrespondingElements(GameObject button, bool active) {
            int index = button.transform.GetSiblingIndex();
            List<GameObject> correspondingObjects = _correspondingObjects[index];

            if (correspondingObjects.Count == 0) return;

            foreach (GameObject correspondingObject in correspondingObjects) {
                correspondingObject.SetActive(active);
            }
        }
    }
}