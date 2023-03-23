using UnityEngine;
using TurnBasedRPG;
using Zenject;

public class PlayerDetection : MonoBehaviour
{
    private SignalBus _signalBus;
    
    [Inject]
    public void Init(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _signalBus.TryFire(new PlayerDetectedSignal(transform.parent));
        }
    }
}