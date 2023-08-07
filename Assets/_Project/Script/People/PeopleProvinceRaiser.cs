using System.Collections.Generic;
using FakeMG.Utilities;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

namespace FakeMG.People {
    public class PeopleProvinceRaiser : MonoBehaviour {
        [SerializeField] private GameObject vietnamModel;
        
        private readonly List<RaiseObject> _provinceBehaviours = new();

        private void Awake() {
            foreach (Transform province in vietnamModel.transform) {
                _provinceBehaviours.Add(province.GetComponent<RaiseObject>());
            }
        }

        public void RaiseProvinceOfPeople(TextMeshProUGUI peopleName) {
            FirebaseDatabase.DefaultInstance
                .GetReference("people").Child(peopleName.text).Child("Location")
                .GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsFaulted) {
                        Debug.Log("Error retrieving people data");
                    } else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;

                        foreach (RaiseObject provinceRaiser in _provinceBehaviours) {
                            provinceRaiser.LowerProvince();
                        }
                        
                        foreach (DataSnapshot dataSnapshot in snapshot.Children) {
                            RaiseObject raiseObject = _provinceBehaviours.Find(province => province.gameObject.name == dataSnapshot.Key);
                            raiseObject.RaiseProvince();
                        }
                    }
                });
        }
    }
}