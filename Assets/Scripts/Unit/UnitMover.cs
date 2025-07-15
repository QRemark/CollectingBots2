using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 25f;

    private Rigidbody _rigidbody;
    private Vector3 _targetPosition;
    private Vector3 _moveDirection;

    private bool _hasTarget = false;

    private float _yVelosity = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetTarget(Vector3 position)
    {
        _targetPosition = position;
        _hasTarget = true;
    }

    public void ClearTarget()
    {
        _hasTarget = false;
    }

    private void Move()
    {
        if (_hasTarget == false)
            return;

        _moveDirection = (_targetPosition - transform.position).normalized;

        Vector3 velocity = _moveDirection * _baseSpeed;
        Vector3 nextPosition = _rigidbody.position + new Vector3(velocity.x, _yVelosity, velocity.z) * Time.fixedDeltaTime;

        _rigidbody.MovePosition(nextPosition);
    }
}
