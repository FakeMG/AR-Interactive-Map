using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace FakeMG.ScriptableObject {
    [CreateAssetMenu(fileName = "OriginalPosition", menuName = "OriginalPosition", order = 0)]
    public class OriginalPosition : UnityEngine.ScriptableObject {
        public Dictionary<GameObject, Vector3> OriginalLocalPositions { get; private set; }
        
        public void SetOriginalPos(Transform vietnamModel) {
            OriginalLocalPositions = new Dictionary<GameObject, Vector3>();
            foreach (Transform region in vietnamModel) {
                foreach (Transform province in region) {
                    OriginalLocalPositions.Add(province.gameObject, province.localPosition);
                }
            }
        }
        
        public void MoveToOriginalPos() {
            foreach (var province in OriginalLocalPositions) {
                province.Key.transform.DOLocalMove(province.Value, 1f).SetEase(Ease.InOutQuad);
            }
        }
        
        public void ResetOriginalPos() {
            foreach (var province in OriginalLocalPositions) {
                province.Key.transform.localPosition = province.Value;
            }
        }
    }
}