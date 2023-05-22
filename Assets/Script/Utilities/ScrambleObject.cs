using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScrambleObject : MonoBehaviour {
    [SerializeField] private Transform model;
    [SerializeField] private float minRange = 3f;
    [SerializeField] private float maxRange = 6f;
    [SerializeField] private float minimumObjectsDistance = 0.5f;
    [SerializeField] private float animationSpeed = 6f;
    
    private List<GameObject> _objectsList;
    private List<Vector3> _newPositions;

    private void Start() {
        _newPositions = new List<Vector3>();
        _objectsList = new List<GameObject>();
        foreach (Transform child in model) {
            _objectsList.Add(child.gameObject);
        }
    }

    public void RepositionObjectsRandomly() {
        foreach (var child in _objectsList) {
            Vector3 newPosition = GetRandomNonOverlappingPosition();
            newPosition.y = child.transform.position.y;
            _newPositions.Add(newPosition);
            StartCoroutine(Animation(child.transform, newPosition));
        }
    }

    private Vector3 GetRandomNonOverlappingPosition() {
        Vector3 randomPosition;
        bool overlapDetected;

        do {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            randomPosition = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * Random.Range(minRange, maxRange);
            randomPosition += model.position;

            overlapDetected = false;
            for (int i = 0; i < _newPositions.Count; i++) {
                if (Vector3.Distance(randomPosition, _newPositions[i]) < minimumObjectsDistance) {
                    overlapDetected = true;
                    break;
                }
            }
        } while (overlapDetected);

        return randomPosition;
    }

    private IEnumerator Animation(Transform objectToAnimated, Vector3 desiredPos) {
        while (true) {
            objectToAnimated.position = Vector3.Lerp(objectToAnimated.position, desiredPos, Time.deltaTime * animationSpeed);
            if (Vector3.Distance(objectToAnimated.position, desiredPos) <= 0.01f) {
                objectToAnimated.position = desiredPos;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}