using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 110;
    [SerializeField] private ScanEffect _scanEffect;

    public List<Resource> ScanAvailableResources(Dictionary<Resource, Unit> activeTasks)
    {
        _scanEffect?.Play(_scanRadius);

        List<Resource> availableResources = new();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out Resource resource))
            {
                bool notAssigned = activeTasks.ContainsKey(resource) == false;

                if (resource.IsAvailable && notAssigned)
                {
                    availableResources.Add(resource);
                }
            }
        }

        return availableResources.OrderBy(resource => Vector3.Distance(transform.position, resource.transform.position)).ToList();
    }
}