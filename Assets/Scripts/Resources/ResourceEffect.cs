using UnityEngine;

public class ResourceEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _smokeEffect;

    public void Play()
    {
        if (_smokeEffect == null)
            return;

        _smokeEffect.Clear();
        _smokeEffect.Play();
    }

    public void Stop()
    {
        if (_smokeEffect == null)
            return;

        _smokeEffect.Stop();
    }
}
