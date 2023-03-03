using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rhythm;

[CreateAssetMenu(fileName = "RhythmLevel", menuName = "ScriptableObjects/Rhythm/RhythmLevel")]
public class RhythmLevel : ScriptableObject
{
    public AudioClip Song;
    public Image CoverArt;
    public float BPM;
    public float RollingDuration;
    [HideInInspector] public List<BeatInfo> Beats; // Custom Editor override

    public void AddBeat(BeatInfo beat)
    {
        Beats.Add(beat);
    }
}

[System.Serializable]
public struct BeatInfo
{
    public Column Column;
    public float Beat; // in Beat counts
    public string Lyrics;
}