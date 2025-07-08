using UnityEngine;
using System;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _parentPosition;

    [SerializeField] private int _initialSize = 10;
    [SerializeField] private int _maxSize = 20;

    private Pool<T> _pool;

    protected Pool<T> Pool => _pool;
    protected T Prefab => _prefab; 

    public int ActiveObjectCount => _pool.ActiveCount;
    public int TotalCreated => _pool.TotalCreated;
    public int TotalSpawned { get; private set; }

    public event Action CounterUpdated;

    protected virtual void Start()
    {
        if (_pool == null)
        {
            _pool = new Pool<T>();
            _pool.Initialize(_prefab, _initialSize, _maxSize);
            _pool.SetParent(_parentPosition);
            _pool.PoolChanged += UpdateCounters;
        }
    }

    protected T SpawnObject(Vector3 position, Quaternion rotation)
    {
        T obj = _pool.GetObject(true);

        if(obj == null)
            return null;

        obj.transform.SetPositionAndRotation(position, rotation);

        TotalSpawned++;
        UpdateCounters();

        return obj;
    }

    public void ReturnToPool(T obj)
    {
        _pool.ReleaseObject(obj);
    }

    protected virtual void UpdateCounters()
    {
        CounterUpdated?.Invoke();
    }
}
