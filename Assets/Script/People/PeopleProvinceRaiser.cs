using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

namespace Script.People {
    public class PeopleProvinceRaiser : MonoBehaviour {
        [SerializeField] GameObject vietnamModel;
        
        private readonly List<ProvinceRaiser> _provinceBehaviours = new();

        private void Start() {
            foreach (Transform province in vietnamModel.transform) {
                _provinceBehaviours.Add(province.GetComponent<ProvinceRaiser>());
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

                        foreach (ProvinceRaiser provinceRaiser in _provinceBehaviours) {
                            provinceRaiser.LowerProvince();
                        }
                        
                        foreach (DataSnapshot dataSnapshot in snapshot.Children) {
                            ProvinceRaiser provinceRaiser = _provinceBehaviours.Find(province => province.gameObject.name == dataSnapshot.Key);
                            provinceRaiser.RaiseProvince();
                        }
                    }
                });
        }
    }
}