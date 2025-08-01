using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _parentPosition;
    [SerializeField] private int _initialSize = 10;

    private Pool<T> _pool;

    protected virtual void Start()
    {
        _pool = new Pool<T>();
        _pool.SetParent(_parentPosition);
        _pool.Initialize(_prefab, _initialSize);
    }

    protected T SpawnObject(Vector3 position, Quaternion rotation)
    {
        T obj = _pool.GetObject();

        if (obj == null)
            return null;

        obj.transform.SetPositionAndRotation(position, rotation);

        return obj;
    }

    public virtual void ReturnToPool(T obj)
    {
        _pool.ReleaseObject(obj);
    }
}