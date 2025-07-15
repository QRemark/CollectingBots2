using System;
using UnityEngine;

[RequireComponent(typeof(UnitMover))]
[RequireComponent(typeof(UnitResourceHandler))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _pickupRadius = 10f;

    private UnitMover _mover;
    private UnitResourceHandler _resourceHandler;

    private Vector3 _basePosition;
    private Resource _targetResource;
    private bool _isBusy;

    public bool ReadyForNewTask { get; private set; }
    public bool IsBusy => _isBusy;
    public UnitMover Mover => _mover;
    public Vector3 BasePosition => _basePosition;

    public event Action<Unit, Resource> ResourceDelivered;

    public void Initialize(Vector3 basePos)
    {
        _basePosition = basePos;
        _isBusy = false;
        ReadyForNewTask = true;
    }

    private void Awake()
    {
        _mover = GetComponent<UnitMover>();
        _resourceHandler = GetComponent<UnitResourceHandler>();
    }

    private void FixedUpdate()
    {
        if (_isBusy == false)
            return;

        if (_resourceHandler.IsCarrying == false)
        {
            _resourceHandler.TryPickupPhase(_targetResource, _pickupRadius, this);
        }
        else
        {
            _resourceHandler.TryDeliveryPhase(_basePosition, this);
        }
    }

    public bool SetTarget(Resource resource)
    {
        if (resource == null)
        {
            return false;
        }

        if (_isBusy)
        {
            return false;
        }

        _targetResource = resource;
        _mover.SetTarget(resource.transform.position);
        _resourceHandler.SetCarriedResource(null);
        _resourceHandler.ClearCarryState();
        _isBusy = true;
        ReadyForNewTask = false;

        resource.MarkReserved();

        return true;
    }

    public void NotifyDelivery(Resource delivered)
    {
        ResourceDelivered?.Invoke(this, delivered);
    }

    public void BecomeIdle()
    {
        _targetResource = null;
        _isBusy = false;
        ReadyForNewTask = true;
        _mover.ClearTarget();
    }
}