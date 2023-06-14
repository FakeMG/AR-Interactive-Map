using UnityEngine;

public class ScaleObject : MonoBehaviour {
    [SerializeField] private float speed = 6f;
    [SerializeField] private Vector3 min = Vector3.zero;
    [SerializeField] private Vector3 max = Vector3.one;

    private Vector3 _desiredScale = Vector3.zero;

    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, _desiredScale, Time.deltaTime * speed);
        if (Vector3.Distance(transform.localScale, _desiredScale) <= 0.001f) {
            transform.localScale = _desiredScale;
        }
    }

    public void ScaleUp() {
        _desiredScale = max;
    }

    public void ScaleDown() {
        _desiredScale = min;
    }

    public bool IsUp() {
        return _desiredScale == max;
    }
}