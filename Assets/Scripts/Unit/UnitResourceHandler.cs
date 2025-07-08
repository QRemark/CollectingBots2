using UnityEngine;

public class UnitResourceHandler : MonoBehaviour
{
    [SerializeField] private float _pickupHeightOffset = 6f;
    [SerializeField] private float _deliveryRadius = 2f;

    private Resource _carriedResource;
    public bool IsCarrying => _carriedResource != null;

    public void SetCarriedResource(Resource resource)
    {
        _carriedResource = resource;
    }

    public void ClearCarryState()
    {
        _carriedResource = null;
    }

    public void TryPickupPhase(Resource targetResource, float pickupRadius, Unit unit)
    {
        if (Vector3.Distance(unit.transform.position, targetResource.transform.position) > pickupRadius)
            return;

        targetResource.MarkCollected();
        _carriedResource = targetResource;

        Rigidbody rigiedbody = _carriedResource.GetComponent<Rigidbody>();

        if (rigiedbody != null)
        {
            rigiedbody.isKinematic = true;
        }

        _carriedResource.transform.SetParent(unit.transform);
        _carriedResource.transform.localPosition = new Vector3(0, _pickupHeightOffset, 0);

        unit.GetMover().SetTarget(unit.GetBasePosition());
    }

    public void TryDeliveryPhase(Vector3 basePosition, UnitMover mover, Unit unit)
    {
        float distance = Vector3.Distance(unit.transform.position, basePosition);

        if (distance > _deliveryRadius)
            return;

        if (_carriedResource != null)
        {
            _carriedResource.transform.SetParent(null);

            Rigidbody rigiedbody = _carriedResource.GetComponent<Rigidbody>();

            if (rigiedbody != null)
                rigiedbody.isKinematic = false;

            Resource delivered = _carriedResource;
            _carriedResource = null;

            unit.NotifyDelivery(delivered);
        }

        mover.ClearTarget();
        unit.BecomeIdle();
    }

}