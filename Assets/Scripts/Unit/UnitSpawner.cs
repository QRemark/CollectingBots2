using UnityEngine;
using System.Collections.Generic;

public class UnitSpawner : Spawner<Unit>
{
    [SerializeField] private Vector3 _spawnPoint;
    [SerializeField] private int _initialUnitCount = 3;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _angleStepDegrees = 30f;

    private float _yOffset = 0f; 

    private List<Unit> _units = new List<Unit>();
    public IReadOnlyList<Unit> Units => _units;


    protected override void Start()
    {
        if (_spawnPoint == Vector3.zero)
            _spawnPoint = transform.position;

        base.Start();
        SpawnInitialUnits();
    }

    private void SpawnInitialUnits()
    {
        for (int i = 0; i < _initialUnitCount; i++)
        {
            float angle = i * _angleStepDegrees * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * _spawnRadius;
            float z = Mathf.Sin(angle) * _spawnRadius;

            Vector3 offset = new Vector3(x, _yOffset, z);
            Vector3 spawnPosition = _spawnPoint + offset;

            Unit unit = SpawnObject(spawnPosition, Quaternion.identity);

            if (unit != null)
            {
                var rb = unit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.position = spawnPosition;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
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