using System;
using System.Collections.Generic;
using UnityEngine;

public class TapBeat : MonoBehaviour, IBeat
{
    public float SecondsBehindBeat { get; set; }
    public Action<GameObject> Event_Recycle { get; set; }
    public int Id { get; set; }
    public BeatInfo Info { get; set; }
    private float t;
    private float _duration;
    private Vector3 _startPoint, _endPoint, _startScale, _endScale;
    private bool _reached;

    public void OnSpawned(Dictionary<string, object> args)
    {
        t = 0;
        _reached = false;
        _duration = (float)args["Duration"];
        SecondsBehindBeat = -_duration;
        _startPoint = gameObject.transform.position;
        _endPoint = (Vector3)args["EndPoint"];
        _startScale = Vector3.one;
        _endScale = Vector3.one * 8;
    }

    private void Update()
    {
        if (_reached) {
            SecondsBehindBeat = t * _duration;
        } else {
            SecondsBehindBeat = (t-1) * _duration;
            transform.localScale = Vector3.Lerp(_startScale, _endScale, t);
        }

        if (t >= 1) {
            SecondsBehindBeat = 0;
            _reached = true;
            t = 0;
            Vector3 sPos = _startPoint;
            _startPoint = _endPoint;
            _endPoint = _endPoint + _endPoint - sPos;

            // GetComponent<SpriteRenderer>().color = Color.green;
        }

        t += Time.deltaTime / _duration;
        transform.position = Vector3.Lerp(_startPoint, _endPoint, t);
    }

    private void OnBecameInvisible()
    {
        OnDespawned();
    }

    public void OnDespawned()
    {
        // GetComponent<SpriteRenderer>().color = Color.white;
        Event_Recycle.Invoke(gameObject);
    }
}
