using System.Collections.Generic;
using FakeMG.Utilities;
using UnityEngine;

namespace FakeMG.Province {
    public class ProvinceController : MonoBehaviour {
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
    }
}