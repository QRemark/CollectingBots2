using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ResourceEffect _resourceEffect;

    private bool _isAvailable;

    public bool IsAvailable => _isAvailable;
    public bool IsReserved { get; private set; } = false;

    public void Activate(Vector3 position)
    {
        transform.position = position;
        _isAvailable = true;
        _resourceEffect?.Play();
    }

    public void MarkCollected()
    {
        _isAvailable = false;
        IsReserved = false;
        _resourceEffect?.Stop();
    }

    public void ResetState()
    {
        _isAvailable = false;
        IsReserved = false;
        _resourceEffect?.Stop();
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
