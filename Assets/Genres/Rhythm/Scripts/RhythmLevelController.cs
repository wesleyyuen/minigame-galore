using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rhythm;

public class RhythmLevelController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private RhythmLevel _level;
    public Dictionary<Column, List<IBeat>> Beats {get;} = new Dictionary<Column, List<IBeat>>();
    public event Action<RhythmLevel> Event_StartRhythmLevel, Event_EndRhythmLevel;

    private void Awake()
    {
        foreach (var col in Enum.GetValues(typeof(Column))) {
            Beats[(Column)col] = new List<IBeat>();
        }
        
        StartCoroutine(_StartRhythmLevel(_level));
    }

    private IEnumerator _StartRhythmLevel(RhythmLevel level)
    {
        _audioSource.clip = level.Song;

        yield return new WaitForSeconds(3);

        Event_StartRhythmLevel.Invoke(level);
        // _metronome.StartMetronome(level.BPM);
        // _spawner.StartSpawnSequence(sorted, level.BPM, level.RollingDuration);

        // Start Audio
        _audioSource.Play();

        yield return new WaitForSeconds(level.Song.length);

        Event_EndRhythmLevel.Invoke(level);
    }

    // public void AddBeat(Column col)
    // {
    //     _level.AddBeat(new BeatInfo{
    //         Column = col,
    //         Beat = _metronome.BeatCount
    //     });
    // }
}
