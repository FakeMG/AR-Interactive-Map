using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScrambleObject : MonoBehaviour {
    [SerializeField] private Transform model;
    [SerializeField] private float minRange = 3f;
    [SerializeField] private float maxRange = 6f;
    [SerializeField] private float minimumObjectsDistance = 0.5f;
    private List<GameObject> _objectsList;

    private void Start() {
        _objectsList = new List<GameObject>();
        foreach (Transform child in model) {
            _objectsList.Add(child.gameObject);
        }
        
        RepositionObjectsRandomly();
    }

    public void RepositionObjectsRandomly() {
        foreach (var t in _objectsList) {
            Vector3 newPosition = GetRandomNonOverlappingPosition();
            newPosition.y = t.transform.position.y;
            t.transform.position = newPosition;
            return;
        }
    }

    private Vector3 GetRandomNonOverlappingPosition() {
        Vector3 randomPosition;
        bool overlapDetected;

        do {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            randomPosition = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * Random.Range(minRange, maxRange);

            overlapDetected = false;
            for (int i = 0; i < _objectsList.Count; i++) {
                if (Vector3.Distance(randomPosition, _objectsList[i].transform.position) < minimumObjectsDistance) {
                    overlapDetected = true;
                    break;
                }
            }
        } while (overlapDetected);

        return randomPosition;
    }
}