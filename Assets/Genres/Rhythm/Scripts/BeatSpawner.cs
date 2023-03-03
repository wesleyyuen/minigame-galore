using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rhythm;
using System.Linq;

public class BeatSpawner : ObjectPool
{
    [SerializeField] private RhythmLevelController _levelController;
    private Camera _camera;
    private float _beatCount;
    private bool _startBeatCount;

    private void OnEnable()
    {
        _levelController.Event_StartRhythmLevel += StartSpawnSequence;
    }

    private void OnDisable()
    {
        _levelController.Event_StartRhythmLevel += StartSpawnSequence;
    }

    public void StartSpawnSequence(RhythmLevel level)
    {
        _camera = Camera.main;

        StopAllCoroutines();

        var sorted = level.Beats.OrderBy(b => b.Beat).ToList();
        StartCoroutine(Spawn(sorted, level.BPM, level.RollingDuration));
    }

    private IEnumerator Spawn(List<BeatInfo> beats, float bpm, float duration)
    {
        float lastBeat = duration / 60f * bpm;
        for (int i = 0; i < beats.Count; ++i)
        {
            BeatInfo beat = beats[i];

            yield return new WaitForSeconds(60f / bpm * (beat.Beat-lastBeat));

            Vector3 spawnPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2,Screen.height));
            spawnPoint.z = 0f;

            Vector3 endPoint = Camera.main.ScreenToWorldPoint(new Vector3(
                Screen.width * (beat.Column == Column.Center ? 0.5f : ((beat.Column == Column.Left ? -1 : 1) * Constants.COLUMN_GAP + 0.5f)),
                Screen.height * Constants.BEAT_HIT_HEIGHT));
            endPoint.z = 0f;

            GameObject beatGO = GetByIdFromPool("Beat", spawnPoint, Quaternion.identity, new Dictionary<string, object>{
                {"EndPoint", endPoint},
                {"Duration", duration <= 0 ? 1 : duration}
            }, go => {
                if (go.TryGetComponent<IBeat>(out IBeat b)) {
                    b.Id = i;
                    b.Info = beat;
                    b.Event_Recycle += Recycle;

                    _levelController.Beats[beat.Column].Add(b);
                }
            });

            lastBeat = beat.Beat;
        }
    }

    public override void Recycle(GameObject obj)
    {
        if (obj.TryGetComponent<IBeat>(out IBeat b)) {
            _levelController.Beats[b.Info.Column].Remove(b);
        }

        base.Recycle(obj);
    }
}
