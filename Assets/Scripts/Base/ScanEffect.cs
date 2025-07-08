using UnityEngine;

public class ScanEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _scanEffect;

    public void Play(float radius)
    {
        if (_scanEffect == null) return;

        var shape = _scanEffect.shape;
        shape.radius = radius;

        _scanEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _scanEffect.Play();
    }
}
