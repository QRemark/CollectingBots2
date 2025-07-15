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

    public event Action<Unit, Resource> ResourceDelivered;

    public void Initialize(Vector3 position)
    {
        _basePosition = position;
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

        if (_resourceHandler.IsTryPickup(_targetResource, _pickupRadius))
        {
            _mover.SetTarget(_basePosition);
        }
        if (_resourceHandler.IsTryDelivery(_basePosition, out Resource delivered))
        {
            NotifyDelivery(delivered);
            BecomeIdle();
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