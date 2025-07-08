using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [SerializeField] private float _spawnInterval = 1f;

    [SerializeField] private float _innerRadius = 20f;
    [SerializeField] private float _outerRadius = 100f;

    [SerializeField] private float _spawnYOffcet = 20f;

    [SerializeField] private Transform _spawnCenter;

    private float _timer;

    private void Update()
    {
        GenerateResource();
    }

    private void GenerateResource()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            _timer = 0f;
            SpawnResource();
        }
    }

    private void SpawnResource()
    {
        Vector3 position = GetRandomPosition();
        Resource resource = SpawnObject(position, Quaternion.identity);

        if (resource != null)
        {
            resource.Activate(position);
        }
    }

    private Vector3 GetRandomPosition()
    {
        const float FullCircleRadians = Mathf.PI * 2f;

        float angle = Random.Range(0f, FullCircleRadians);
        float radius = Mathf.Sqrt(Random.Range(_innerRadius * _innerRadius, _outerRadius * _outerRadius));

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        return new Vector3(_spawnCenter.position.x + x, _spawnYOffcet, _spawnCenter.position.z + z);
    }
}