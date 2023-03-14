using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour {
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    public Camera arCamera;
    public ARRaycastManager arRaycastManager;
    public float holdTime = 2;
    [SerializeField] private LayerMask layerMask;
    
    private Pose _placementPose;
    private bool _placementPoseIsValid;

    private float _timeCounter;

    private void Update() {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (CanBePlaced() || CanBeDeactivated()) {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary) {
                _timeCounter += Time.deltaTime;
                if (_timeCounter >= holdTime) {
                    ToggleObject();
                    _timeCounter = 0;
                }
            } else {
                _timeCounter = 0;
            }
        } else {
            _timeCounter = 0;
        }
    }

    private bool CanBePlaced() {
        return !objectToPlace.activeSelf && _placementPoseIsValid;
    }

    private bool CanBeDeactivated() {
        var cameraTransform = arCamera.transform;
        return objectToPlace.activeSelf &&
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
        if (_placementPoseIsValid) {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        } else {
            placementIndicator.SetActive(false);
        }
    }

    private void ToggleObject() {
        objectToPlace.SetActive(!objectToPlace.activeSelf);
        objectToPlace.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
    }
}