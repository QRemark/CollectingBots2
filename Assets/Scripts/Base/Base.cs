using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceScanScheduler _scanner;

    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private TaskAssigner _assigner;

    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private BaseResourceUI _resourceUI;

    [SerializeField] private ResourceStorage _resourceStorage;

    private Dictionary<Resource, Unit> _activeTasks;
    private List<Unit> _subscribedUnits;
    private List<Resource> _availableResources;

    private void Awake()
    {
        _activeTasks = new Dictionary<Resource, Unit>();
        _subscribedUnits = new List<Unit>();
        _availableResources = new List<Resource>();
    }

    private void Start()
    {
        _assigner.Init(_activeTasks, _resourceStorage);
        _resourceUI.Initialize(_resourceCounter);
        _scanner.ResourcesUpdated += OnResourcesUpdated;
    }

    private void OnResourcesUpdated(List<Resource> scannedResources)
    {
        CheckUnits();

        _availableResources = new List<Resource>();
        _availableResources = FilterAvailableResources(scannedResources);

        _assigner.AssignTasks(_unitSpawner.Units, _availableResources);
    }

    private List<Resource> FilterAvailableResources(List<Resource> scannedResources)
    {
        return scannedResources
            .Where(resource => _resourceStorage.AvailableResources.Contains(resource))
            .ToList();
    }

    private void CheckUnits()
    {
        foreach (Unit unit in _unitSpawner.Units)
        {
            if (_subscribedUnits.Contains(unit) == false)
            {
                unit.ResourceDelivered += OnUnitDelivered;
                _subscribedUnits.Add(unit);
            }
        }
    }

    private void OnUnitDelivered(Unit unit, Resource resource)
    {
        if (_activeTasks.ContainsKey(resource))
        {
            _activeTasks.Remove(resource);
        }

        _resourceStorage.UnregisterResource(resource);
        _resourceCounter.Increment();

        resource.Collect();
    }

    private void OnDestroy()
    {
        _scanner.ResourcesUpdated -= OnResourcesUpdated;

        foreach (Unit unit in _subscribedUnits)
        {
            unit.ResourceDelivered -= OnUnitDelivered;
        }
    }
}
