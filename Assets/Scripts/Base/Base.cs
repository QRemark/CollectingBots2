using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private float _scanInterval = 5f;

    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private ResourceSpawner _resourceSpawner;

    [SerializeField] private BaseResourceScanner _scanner;
    [SerializeField] private TaskAssigner _assigner;

    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private BaseResourceUI _resourceUI;

    private Dictionary<Resource, Unit> _activeTasks;
    private List<Unit> _subscribedUnits;
    private List<Resource> _availableResources;
    public ResourceCounter ResourceCounter => _resourceCounter;

    private void Awake()
    {
        _activeTasks = new Dictionary<Resource, Unit>();
        _subscribedUnits = new List<Unit>();
        _availableResources = new List<Resource>();
    }

    private void Start()
    {
        _assigner.Init(_activeTasks);
        SubscribeAllUnits();
        _resourceUI.Initialize(_resourceCounter);
        StartCoroutine(ScanRoutine());
    }

    private IEnumerator ScanRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_scanInterval);
            PerformScanCycle();
        }
    }

    private void PerformScanCycle()
    {
        SubscribeAllUnits();
        RefreshAvailableResources();

        _assigner.AssignTasks(_unitSpawner.Units, _availableResources);
    }

    private void SubscribeAllUnits()
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

    private void RefreshAvailableResources()
    {
        _availableResources = _scanner.ScanAvailableResources(_activeTasks);
    }

    private void OnUnitDelivered(Unit unit, Resource resource)
    {
        if (_activeTasks.ContainsKey(resource))
        {
            _activeTasks.Remove(resource);
        }

        _resourceCounter.Increment();

        resource.ResetState();
        _resourceSpawner.ReturnToPool(resource);
    }
}
