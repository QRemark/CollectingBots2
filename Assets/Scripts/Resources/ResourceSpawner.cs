using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private float _spawnRadius = 10f;
    [SerializeField] private float _spawnYOffcet = 7f;
    [SerializeField] private Vector3 _spawnCenter;

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
        _spawnCenter.y = _spawnYOffcet;
        Vector2 circle = Random.insideUnitCircle * _spawnRadius;
        return new Vector3 (_spawnCenter.x + circle.x, _spawnCenter.y, _spawnCenter.z + circle.y);
    }
}
