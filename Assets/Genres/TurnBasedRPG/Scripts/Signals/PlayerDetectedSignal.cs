using UnityEngine;

namespace TurnBasedRPG
{
public class PlayerDetectedSignal
{
    public Transform Detector { get; }
    public PlayerDetectedSignal(Transform detector)
    {
        Detector = detector;
    }
}
}

