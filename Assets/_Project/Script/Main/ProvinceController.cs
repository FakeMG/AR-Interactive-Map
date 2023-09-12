using System.Collections.Generic;
using UnityEngine;

namespace FakeMG.Main {
    public class ProvinceController : MonoBehaviour {
        [SerializeField] private GameObject vietnamModel;
        private List<RaiseObject> _provinceRaisers;

        private void Awake() {
            RaiseObject[] provinceRaisers = FindObjectsOfType<RaiseObject>();
            _provinceRaisers = new List<RaiseObject>(provinceRaisers);
        }
        
        public void LowerAllProvince() {
            foreach (RaiseObject province in _provinceRaisers) {
                province.LowerProvince();
            }
        }

        public void ShowAllRegion() {
            foreach (Transform region in vietnamModel.transform) {
                region.gameObject.SetActive(true);
            }
        }
    }
}