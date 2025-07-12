using UnityEngine;
using System.Collections.Generic;

public class UnitSpawner : Spawner<Unit>
{
    [SerializeField] private Vector3 _spawnPoint;
    [SerializeField] private int _initialUnitCount = 3;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _angleStepDegrees = 30f;

    private float _yOffset = 0f; 
    private Quaternion _defaultRotation = Quaternion.identity;
    private Vector3 _defaultVelocity = Vector3.zero;
    private List<Unit> _units = new List<Unit>();

    public IReadOnlyList<Unit> Units => _units;

    protected override void Start()
    {
        if (_spawnPoint == _defaultVelocity)
            _spawnPoint = transform.position;

        base.Start();
        CreateInitialUnits();
    }

    private void CreateInitialUnits()
    {
        for (int i = 0; i < _initialUnitCount; i++)
        {
            float angle = i * _angleStepDegrees * Mathf.Deg2Rad;
            float offsetX = Mathf.Cos(angle) * _spawnRadius;
            float offsetZ = Mathf.Sin(angle) * _spawnRadius;

            Vector3 offset = new Vector3(offsetX, _yOffset, offsetZ);
            Vector3 spawnPosition = _spawnPoint + offset;

            Unit unit = SpawnObject(spawnPosition, _defaultRotation);

            if (unit != null)
            {
                if (unit.TryGetComponent(out Rigidbody unitRigidbody))
                {
                    unitRigidbody.position = spawnPosition;
                    unitRigidbody.linearVelocity = _defaultVelocity;
                    unitRigidbody.angularVelocity = _defaultVelocity;
                }
                else
                {
                    unit.transform.position = spawnPosition;
                }

                unit.Initialize(transform.position);
                _units.Add(unit);
            }
        }
    }
}