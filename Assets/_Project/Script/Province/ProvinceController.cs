using System.Collections.Generic;
using FakeMG.Utilities;
using UnityEngine;

namespace FakeMG.Province {
    public class ProvinceController : MonoBehaviour {

        private readonly List<RaiseObject> _provinceRaisers = new();

        private void Awake() {
            RaiseObject[] provinceRaisers = FindObjectsOfType<RaiseObject>();
            _provinceRaisers.AddRange(provinceRaisers);
        }
        
        public void LowerAllProvinceExcept(RaiseObject province) {
            foreach (RaiseObject provinceRaiser in _provinceRaisers) {
                if (provinceRaiser != province) {
                    provinceRaiser.LowerProvince();
                }
            }
        }
        
        public void LowerAllProvince() {
            foreach (RaiseObject province in _provinceRaisers) {
                province.LowerProvince();
            }
        }
    }
}