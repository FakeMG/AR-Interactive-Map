using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckWin : MonoBehaviour {
    [SerializeField] private Transform vietnamModel;
    [SerializeField] private ScaleObject winUI;
    private Dictionary<GameObject, Vector3> _originalPositions;

    private void Start() {
        //TODO: remove this later
        GetOriginalPos();
    }

    private void Update() {
        if (IsWin()) {
            winUI.ScaleUp();
        }
    }

    private bool IsWin() {
        return _originalPositions.All(item => item.Key.transform.position == item.Value);
    }
    
    public void GetOriginalPos() {
        _originalPositions = new Dictionary<GameObject, Vector3>();
        foreach (Transform child in vietnamModel) {
            _originalPositions.Add(child.gameObject, child.position);
        }
    }
}