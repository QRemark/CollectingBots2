using System.Collections.Generic;
using System;
using UnityEngine;

public class Pool<T> where T: MonoBehaviour
{
    private Queue<T> _deactiveObjects = new Queue<T>();
    private List<T> _activeObjects = new List<T>();
    
    private T _prefab;
    
    private Transform _parent;
    
    private int _maxSize;
    private int _curreentCount;

    public event Action PoolChanged;

    public int TotalCreated { get; private set; }
    public int ActiveCount => _activeObjects.Count;

    public void Initialize(T prefab, int initalSize, int maxSize)
    {
        _prefab = prefab;
        _maxSize = maxSize;
        _curreentCount = 0;

        TotalCreated = 0;

        _deactiveObjects.Clear();
        _activeObjects.Clear();

        for (int i = 0; i < initalSize; i++)
        {
            T obj = Create();
            
            if(obj != null)
            {
                obj.gameObject.SetActive(false);
                _deactiveObjects.Enqueue(obj);
            }
        }
    }

    public void SetParent(Transform parent)
    {
        _parent = parent;
    }

    public T GetObject(bool activate = true)
    {
        if (_deactiveObjects.Count > 0)
        {
            T obj = _deactiveObjects.Dequeue();

            if(activate)
                obj.gameObject.SetActive(true);

            _activeObjects.Add(obj);
            PoolChanged?.Invoke();

            return obj;
        }

        T newObj = Create();

        if (newObj != null)
        {
            if (activate)
                newObj.gameObject.SetActive(true);

            _activeObjects.Add(newObj);
            PoolChanged?.Invoke();

            return newObj;
        }
        
        return null;
    }

    public void ReleaseObject(T obj)
    {
        if (_deactiveObjects.Contains(obj) == false)
        {
            obj.gameObject.SetActive(false);
            _deactiveObjects.Enqueue(obj);
            _activeObjects.Remove(obj);
            PoolChanged?.Invoke();
        }
    }

    private T Create()
    {
        if (_curreentCount >= _maxSize)
        {
            return null;
        }

        T obj = UnityEngine.Object.Instantiate(_prefab, _parent);
        _curreentCount++;
        TotalCreated++;

        return obj;
    }
}
