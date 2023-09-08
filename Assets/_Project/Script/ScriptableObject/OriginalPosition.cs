using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace FakeMG.ScriptableObject {
    [CreateAssetMenu(fileName = "OriginalPosition", menuName = "OriginalPosition", order = 0)]
    public class OriginalPosition : UnityEngine.ScriptableObject {
        public Dictionary<GameObject, Vector3> OriginalPositions { get; private set; }
        
        public void SetOriginalPos(Transform vietnamModel) {
            OriginalPositions = new Dictionary<GameObject, Vector3>();
            foreach (Transform region in vietnamModel) {
                foreach (Transform province in region) {
                    OriginalPositions.Add(province.gameObject, province.position);
                }
            }
        }
        
        public void MoveToOriginalPos() {
            foreach (var province in OriginalPositions) {
                province.Key.transform.DOMove(province.Value, 1f).SetEase(Ease.InOutQuad);
            }
        }
    }
}