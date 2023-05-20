using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlaceObject : MonoBehaviour {
    [SerializeField] private  GameObject objectToPlace;
    [SerializeField] private  GameObject placementIndicator;
    [SerializeField] private  Camera arCamera;
    [SerializeField] private  ARRaycastManager arRaycastManager;
    [SerializeField] private  float holdTime = 2;
    [SerializeField] private float animationSpeed = 6f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private UnityEvent onPlacedObject;

    private Pose _placementPose;
    private bool _placementPoseIsValid;

    private float _timeCounter;
    private Vector3 _desiredObjectScale;

    private void Start() {
        _desiredObjectScale = Vector3.one;
        placementIndicator.SetActive(true);
    }

    private void Update() {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (CanBeDeactivated() && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary) {
            _timeCounter += Time.deltaTime;
            if (_timeCounter >= holdTime) {
                ToggleTargetObject();
                _timeCounter = 0;
            }
        } else {
            _timeCounter = 0;
        }

        if (CanBePlaced()) {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                ToggleTargetObject();
                onPlacedObject?.Invoke();
            }
        }

        if (objectToPlace != null) {
            objectToPlace.transform.localScale =
                Vector3.Lerp(objectToPlace.transform.localScale, _desiredObjectScale, Time.deltaTime * animationSpeed);
            if (Vector3.Distance(objectToPlace.transform.localScale, _desiredObjectScale) <= 0.01f) {
                objectToPlace.transform.localScale = _desiredObjectScale;
            }
        }

        objectToPlace.SetActive(objectToPlace.transform.localScale != Vector3.zero);
    }

    private bool CanBePlaced() {
        return _desiredObjectScale == Vector3.zero && _placementPoseIsValid;
    }

    private bool CanBeDeactivated() {
        var cameraTransform = arCamera.transform;
        return _desiredObjectScale == Vector3.one &&
               Physics.Raycast(cameraTransform.position, cameraTransform.forward, 10, layerMask.value);
    }

    private void UpdatePlacementPose() {
        var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        _placementPoseIsValid = hits.Count > 0;
        if (_placementPoseIsValid) {
            _placementPose = hits[0].pose;

            var cameraForward = arCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            _placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator() {
        if (!objectToPlace.activeSelf) {
            if (_placementPoseIsValid) {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
            } else {
                placementIndicator.SetActive(false);
            }
        } else {
            placementIndicator.SetActive(false);
        }
    }

    private void ToggleTargetObject() {
        _desiredObjectScale = objectToPlace.activeSelf ? Vector3.zero : Vector3.one;

        if (_desiredObjectScale == Vector3.zero) return;

        objectToPlace.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
    }
}