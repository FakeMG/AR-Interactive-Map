using System.Collections.Generic;
using FakeMG.ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

namespace FakeMG.Utilities {
    public class InitRegionButton : MonoBehaviour {
        [SerializeField] private GameObject vietnamModel;
        [SerializeField] private GameObject vietnamModelWhite;
        [SerializeField] private ScrambleObject scrambleObject;
        [SerializeField] private Transform regionButtonList;
        [SerializeField] private Transform regionButtonUI;
        [SerializeField] private OriginalPosition originalPosition;

        private List<Button> _regionButton;
        private List<GameObject> _region;
        private List<GameObject> _regionWhite;

        private void Awake() {
            _regionButton = new List<Button>(regionButtonList.childCount);
            foreach (Transform button in regionButtonList) {
                _regionButton.Add(button.GetComponent<Button>());
            }

            _region = new List<GameObject>(vietnamModel.transform.childCount);
            foreach (Transform region in vietnamModel.transform) {
                _region.Add(region.gameObject);
            }
            
            _regionWhite = new List<GameObject>(vietnamModelWhite.transform.childCount);
            foreach (Transform region in vietnamModelWhite.transform) {
                _regionWhite.Add(region.gameObject);
            }

            InitButton();
        }

        private void InitButton() {
            foreach (var button in _regionButton) {
                button.onClick.AddListener(() => {
                    foreach (var region in _region) {
                        region.SetActive(_region.IndexOf(region) == _regionButton.IndexOf(button));
                        if (_region.IndexOf(region) == _regionButton.IndexOf(button)) {
                            scrambleObject.RepositionObjectsRandomlyAround(region.transform);
                        }
                    }
                    
                    foreach (var region in _regionWhite) {
                        region.SetActive(_regionWhite.IndexOf(region) == _regionButton.IndexOf(button));
                    }
                    
                    regionButtonUI.gameObject.SetActive(false);
                    originalPosition.MoveToOriginalPos();
                });
            }
        }
    }
}