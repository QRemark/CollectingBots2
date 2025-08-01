using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    private const float FullCircleRadians = Mathf.PI * 2f;

    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _innerRadius = 20f;
    [SerializeField] private float _outerRadius = 100f;
    [SerializeField] private float _spawnYOffcet = 20f;

    [SerializeField] private Transform _spawnCenter;
    [SerializeField] private ResourceStorage _resourceStorage;

    private float _timer;
    private Quaternion _defaultRotation = Quaternion.identity;

    private void Update()
    {
        GenerateResourceOnInterval();
    }

    public override void ReturnToPool(Resource resource)
    {
        resource.OnCollected -= ReturnToPool; 
        base.ReturnToPool(resource);          
    }

    private void GenerateResourceOnInterval()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            _timer = 0f;
            SpawnAndRegisterResource();
        }
    }

    private void SpawnAndRegisterResource()
    {
        Vector3 position = GetRandomPosition();
        Resource resource = SpawnObject(position, _defaultRotation);

        if (resource != null)
        {
            resource.Activate(position);
            resource.OnCollected += ReturnToPool;
            _resourceStorage.RegisterResource(resource);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float angle = Random.Range(0f, FullCircleRadians);
        float radius = Mathf.Sqrt(Random.Range(_innerRadius * _innerRadius, _outerRadius * _outerRadius));

        float xOffcet = Mathf.Cos(angle) * radius;
        float zOffcet = Mathf.Sin(angle) * radius;

        return new Vector3(_spawnCenter.position.x + xOffcet, _spawnYOffcet, _spawnCenter.position.z + zOffcet);
    }
}