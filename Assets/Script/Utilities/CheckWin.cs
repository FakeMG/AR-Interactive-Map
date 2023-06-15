using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CheckWin : MonoBehaviour {
    [SerializeField] private Transform vietnamModel;
    [SerializeField] private UnityEvent onWin;
    private Dictionary<GameObject, Vector3> _originalPositions;

    private void Start() {
        //TODO: remove this later
        // GetOriginalPos();
    }

    private void Update() {
        if (IsWin()) {
            onWin?.Invoke();
        }
    }

    private bool IsWin() {
        if (_originalPositions == null) return false;
        return _originalPositions.All(item => item.Key.transform.position == item.Value);
    }
    
    public void GetOriginalPos() {
        _originalPositions = new Dictionary<GameObject, Vector3>();
        foreach (Transform child in vietnamModel) {
            _originalPositions.Add(child.gameObject, child.position);
        }
    }
}