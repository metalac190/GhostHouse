using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _leftBound = 0;
    [SerializeField] private float _rightBound = 100;

    private bool _right;

    private void FixedUpdate() {
        if (transform.position.x < _leftBound) {
            _right = true;
        }
        else if (transform.position.x > _rightBound) {
            _right = false;
        }

        var pos = transform.position;
        pos.x += _right ? _speed : -_speed;
        transform.position = pos;
    }
}