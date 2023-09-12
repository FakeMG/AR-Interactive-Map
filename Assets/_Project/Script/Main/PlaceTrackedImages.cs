using System;
using System.Collections.Generic;
using FakeMG.ScriptableObject;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace FakeMG.Main {
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class PlaceTrackedImages : MonoBehaviour {
        // List of prefabs to instantiate - these should be named the same
        // as their corresponding 2D images in the reference image library 
        [SerializeField] private GameObject[] arPrefabs;
        [SerializeField] private GameObject vietnamModel;
        [SerializeField] private OriginalPosition originalPosition;

        // Reference to AR tracked image manager component
        private ARTrackedImageManager _trackedImagesManager;


        // Keep dictionary array of created prefabs
        private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new();

        private void Awake() {
            _trackedImagesManager = GetComponent<ARTrackedImageManager>();
#if !UNITY_EDITOR
            vietnamModel.SetActive(false);
#endif

#if UNITY_EDITOR
            originalPosition.SetOriginalPos(vietnamModel.transform);
#endif
        }

        private void OnEnable() {
            _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        private void OnDisable() {
            _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
            foreach (var trackedImage in eventArgs.added) {
                // Get the name of the reference image
                var imageName = trackedImage.referenceImage.name;

                foreach (var curPrefab in arPrefabs) {
                    // Check whether this prefab matches the tracked image name, and that
                    // the prefab hasn't already been created
                    if (string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0 &&
                        !_instantiatedPrefabs.ContainsKey(imageName)) {
                        // Instantiate the prefab, parenting it to the ARTrackedImage
                        // var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                        // _instantiatedPrefabs[imageName] = newPrefab;

                        vietnamModel.SetActive(true);
                        vietnamModel.transform.SetParent(trackedImage.transform);
                        vietnamModel.transform.localPosition = Vector3.zero;
                        originalPosition.SetOriginalPos(vietnamModel.transform);
                        _instantiatedPrefabs[imageName] = vietnamModel;
                    }
                }
            }

            // foreach (var trackedImage in eventArgs.updated) {
            //     _instantiatedPrefabs[trackedImage.referenceImage.name]
            //         .SetActive(trackedImage.trackingState == TrackingState.Tracking);
            // }


            // foreach (var trackedImage in eventArgs.removed) {
            //     // Or, simply set the prefab instance to inactive
            //     _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(false);
            // }
        }
    }
}