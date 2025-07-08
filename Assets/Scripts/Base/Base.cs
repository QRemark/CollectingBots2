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
    [SerializeField] private BaseTaskAssigner _assigner;

    private Dictionary<Resource, Unit> _activeTasks = new();
    private List<Unit> _subscribedUnits = new();
    private List<Resource> _availableResources = new();

    private int _deliveredResourcesCount = 0;

    private void Start()
    {
        _assigner.Init(_activeTasks);
        SubscribeAllUnits();
        StartCoroutine(ScanRoutine());
    }

    private IEnumerator ScanRoutine()
    {
        while (true)
        {
            PerformScanCycle();
            yield return new WaitForSeconds(_scanInterval);
        }
    }

    private void PerformScanCycle()
    {
        SubscribeAllUnits();
        RefreshAvailableResources();

        List<Unit> idleUnits = new List<Unit>(_unitSpawner.Units.ToList());
        _assigner.AssignTasks(idleUnits, _availableResources);
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

        _deliveredResourcesCount++;
        Debug.Log($"Resource delivered! Total delivered: {_deliveredResourcesCount}");

        resource.ResetState();
        _resourceSpawner.ReturnToPool(resource);

        _assigner.AssignTasks(new List<Unit> { unit }, _availableResources);
    }
}
