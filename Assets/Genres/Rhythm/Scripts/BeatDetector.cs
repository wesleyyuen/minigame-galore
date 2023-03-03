using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rhythm;
using Zenject;

public class BeatDetector : MonoBehaviour
{
    private RhythmInput _input;
    [SerializeField] private RhythmLevelController _levelController;

    [Inject]
    public void Init(RhythmInput input)
    {
        _input = input;
    }

    private void OnEnable()
    {
        _input.Event_Beat += OnBeat;
    }

    private void OnDisable()
    {
        _input.Event_Beat -= OnBeat;
    }

    private void OnBeat(Column col)
    {
        IBeat beat = _levelController.Beats[col].Find(b => Mathf.Abs(b.SecondsBehindBeat) < Constants.MAX_SEC_BEHIND_BEAT);
        if (beat != null) {
            beat.OnDespawned();
            Debug.Log(beat.SecondsBehindBeat);
        }

        // _levelController.AddBeat(col);
    }
}
