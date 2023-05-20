using UnityEngine;

public class ScaleObject : MonoBehaviour {
    [SerializeField] private float speed = 6f;

    private Vector3 _desiredScale = Vector3.zero;

    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, _desiredScale, Time.deltaTime * speed);
        if (Vector3.Distance(transform.localScale, _desiredScale) <= 0.01f) {
            transform.localScale = _desiredScale;
        }
    }

    public void ScaleUp() {
        _desiredScale = Vector3.one;
    }

    public void ScaleDown() {
        _desiredScale = Vector3.zero;
    }

    public bool IsUp() {
        return _desiredScale == Vector3.one;
    }
}