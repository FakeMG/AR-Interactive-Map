﻿using UnityEngine;

public class ProvinceBehaviour : MonoBehaviour {
    [SerializeField] private float speed = 6f;

    private Vector3 _desiredPosition;
    private Vector3 _originLocalPosition;

    private void Start() {
        _originLocalPosition = transform.localPosition;
        _desiredPosition = _originLocalPosition;
    }

    private void Update() {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _desiredPosition, Time.deltaTime * speed);
        if (Vector3.Distance(transform.localPosition, _desiredPosition) <= 0.01f) {
            transform.localPosition = _desiredPosition;
        }
    }

    public void RaiseProvince() {
        _desiredPosition = _originLocalPosition + Vector3.up * 0.2f;
    }

    public void LowerProvince() {
        _desiredPosition = _originLocalPosition;
    }

    public bool IsProvinceUp() {
        return _desiredPosition == _originLocalPosition + Vector3.up * 0.2f;
    }
}