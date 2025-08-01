using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    private int _count = 0;
    public int Count => _count;

    public event Action<int> CountChanged;

    private void Start()
    {
        CountChanged?.Invoke(_count);
    }

    public void Increment()
    {
        _count++;
        CountChanged?.Invoke(_count);
    }
}
