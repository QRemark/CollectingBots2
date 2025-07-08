using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool _isAvailable;
    public bool IsAvailable => _isAvailable;
    public bool IsReserved { get; private set; } = false;

    public void Activate(Vector3 position)
    {
        transform.position = position;
        _isAvailable = true;
    }

    public void MarkCollected()
    {
        _isAvailable = false;
        IsReserved = false;
    }

    public void ResetState()
    {
        _isAvailable = false;
        IsReserved = false;
    }

    public void MarkReserved()
    {
        if (IsAvailable == false)
        {
            return;
        }

        _isAvailable = false;
        IsReserved = true;
    }
}
