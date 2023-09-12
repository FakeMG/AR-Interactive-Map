using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FakeMG.Main.UI {
    public class SortList : MonoBehaviour {
        [SerializeField] private GameObject contentHolder;
        private List<TextMeshProUGUI> _childrenText;
        private int _previousChildCount;

        private void Start() {
            _childrenText = new List<TextMeshProUGUI>();

            foreach (Transform child in contentHolder.transform) {
                _childrenText.Add(child.GetChild(1).GetComponent<TextMeshProUGUI>());
            }

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
        
            Sort();
        }

        private void Sort() {
            TextMeshProUGUI[] countOrdered = _childrenText.OrderBy(go => go.text).ToArray();
            for (int i = 0; i < countOrdered.Length; i++) {
                countOrdered[i].transform.parent.SetSiblingIndex(i);
            }
        }
    }
}