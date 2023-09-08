using System;
using System.Collections.Generic;
using System.Linq;
using FakeMG.ScriptableObject;
using UnityEngine;
using UnityEngine.Events;

namespace FakeMG.Utilities {
    public class CheckWin : MonoBehaviour {
        [SerializeField] private UnityEvent onWin;
        [SerializeField] private OriginalPosition originalPosition;

        private List<GameObject> _objectsList;

        private void Start() {
            enabled = false;
        }

        private void Update() {
            if (IsWin()) {
                onWin?.Invoke();
            }
        }

        public void StartTimer(List<GameObject> objectsList) {
            _objectsList = objectsList;
            //TODO: Start timer
            Debug.Log("Start timer");
        }

        private bool IsWin() {
            return _objectsList.All(obj =>
                obj.transform.position == originalPosition.OriginalPositions[obj.gameObject]);
        }
    }
}