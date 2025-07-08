using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 5f;
    [SerializeField] private float _slopeEffectStrength = 0.5f;
    [SerializeField] private float _slopeCheckDistance = 2f;
    [SerializeField] private float _turnSmoothTime = 5f;
    [SerializeField] private LayerMask _terrainMask;

    private Rigidbody _rigidbody;
    private Vector3 _targetPosition;
    private Vector3 _moveDirection;
    
    private bool _hasTarget = false;

    private float _currentSpeedModifier = 1f;
    private float _yVelosity = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void FixedUpdate()
    {
        Move();
        AlignToTerrain();
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
        if (_hasTarget==false) 
            return;

        _moveDirection = (_targetPosition - transform.position).normalized;
        float slopeModifier = CalculateSlopeModifier();
        _currentSpeedModifier = Mathf.Lerp(_currentSpeedModifier, slopeModifier, Time.fixedDeltaTime);

        Vector3 velocity = _moveDirection * _baseSpeed * _currentSpeedModifier;
        Vector3 nextPosition = _rigidbody.position + new Vector3(velocity.x, _yVelosity, velocity.z) * Time.fixedDeltaTime;

        _rigidbody.MovePosition(nextPosition);
    }

    private float CalculateSlopeModifier()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, _slopeCheckDistance, _terrainMask))
        {
            Vector3 normal = hit.normal;
            float slopeAngle = Vector3.Angle(normal, Vector3.up);
            float slopeFactor = 1f - (slopeAngle / 45f) * _slopeEffectStrength;
            return Mathf.Clamp(slopeFactor, 0.4f, 1.2f);
        }

        return 1f;
    }

    private void AlignToTerrain()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, _slopeCheckDistance, _terrainMask))
        {
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, Time.fixedDeltaTime * _turnSmoothTime);
        }
    }
}
