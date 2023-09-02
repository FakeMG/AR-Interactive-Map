using System.Collections.Generic;
using FakeMG.Database;
using FakeMG.Utilities;
using Firebase.Database;
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
            DatabaseBehavior.Instance.LoadData("people/" + peopleName.text + "/Location", snapshot => {
                foreach (RaiseObject provinceRaiser in _provinceBehaviours) {
                    provinceRaiser.LowerProvince();
                }

                foreach (DataSnapshot dataSnapshot in snapshot.Children) {
                    RaiseObject raiseObject = _provinceBehaviours.Find(province =>
                        province.gameObject.name == dataSnapshot.Key);
                    raiseObject.RaiseProvince();
                }
            });
        }
    }
}