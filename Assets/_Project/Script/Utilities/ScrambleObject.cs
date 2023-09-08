using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace FakeMG.Utilities {
    public class ScrambleObject : MonoBehaviour {
        [SerializeField] private float minRange = 3f;
        [SerializeField] private float maxRange = 6f;
        [SerializeField] private float minimumObjectsDistance = 0.5f;
        [SerializeField] private float animationDuration = 2f;
        [SerializeField] private float delayTime;
        [SerializeField] private UnityEvent<List<GameObject>> callback;

        private List<GameObject> _objectsList;
        private List<Vector3> _newPositions;

        public void RepositionObjectsRandomlyAround(Transform parent) {
            _newPositions = new List<Vector3>();
            _objectsList = new List<GameObject>();
            foreach (Transform child in parent) {
                _objectsList.Add(child.gameObject);
            }

            foreach (var child in _objectsList) {
                Vector3 newPosition = GetRandomNonOverlappingPositionAround(parent);
                newPosition.y = child.transform.position.y;
                child.transform.DOMove(newPosition, animationDuration).SetDelay(delayTime).SetEase(Ease.InOutQuad)
                    .OnComplete(() => {
                        if (_objectsList.IndexOf(child) == _objectsList.Count - 1)
                            callback?.Invoke(_objectsList);
                    });
            }
        }

        public void ReRepositionObjectsRandomly() {
            _newPositions = new List<Vector3>();
            
            foreach (var child in _objectsList) {
                Vector3 newPosition = GetRandomNonOverlappingPositionAround(child.transform.parent);
                newPosition.y = child.transform.position.y;
                child.transform.DOMove(newPosition, animationDuration).SetDelay(delayTime).SetEase(Ease.InOutQuad)
                    .OnComplete(() => {
                        if (_objectsList.IndexOf(child) == _objectsList.Count - 1)
                            callback?.Invoke(_objectsList);
                    });
            }
        }

        private Vector3 GetRandomNonOverlappingPositionAround(Transform parent) {
            Vector3 randomPosition;
            bool overlapDetected;

            do {
                float angle = Random.Range(0f, 2f * Mathf.PI);
                randomPosition = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * Random.Range(minRange, maxRange);
                randomPosition += parent.position;

                overlapDetected = false;
                foreach (var position in _newPositions) {
                    if (Vector3.Distance(randomPosition, position) < minimumObjectsDistance) {
                        overlapDetected = true;
                        break;
                    }
                }
            } while (overlapDetected);

            _newPositions.Add(randomPosition);

            return randomPosition;
        }
    }
}