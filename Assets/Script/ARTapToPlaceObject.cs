using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour {
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    public Camera arCamera;
    public ARRaycastManager arRaycastManager;

    private Pose _placementPose;
    private bool _placementPoseIsValid;

    private void Update() {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (_placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            PlaceObject();
        }
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

    private void PlaceObject() {
        Instantiate(objectToPlace, _placementPose.position, _placementPose.rotation);
    }
}
