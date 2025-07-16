using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceDetector))]
public class ResourceScanScheduler : MonoBehaviour
{
    [SerializeField] private float _scanInterval = 5f;

    public event Action<List<Resource>> ResourcesUpdated;

    private ResourceDetector _detector;

    private void Awake()
    {
        _detector = GetComponent<ResourceDetector>();
    }

    private void Start()
    {
        StartCoroutine(ScanRoutine());
    }

    private IEnumerator ScanRoutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(_scanInterval);
            PerformScan();
        }
    }

    private void PerformScan()
    {
        List<Resource> scannedResources = _detector.DetectNearbyResources();
        ResourcesUpdated?.Invoke(scannedResources);
    }
}
