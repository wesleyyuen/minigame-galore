using UnityEngine;

public class Metronome : MonoBehaviour
{
    public double BeatCount {get; set;} 
    private double _bpm, _nextTick, _buffer = 0.2d;
    private int flip = 0;
    [SerializeField] private AudioSource[] _source = new AudioSource[2];

    public void StartMetronome(float bpm)
    {
        _bpm = (double)bpm;
        _nextTick = AudioSettings.dspTime + 60.0d / _bpm;
    }

    private void FixedUpdate()
    {
        if (AudioSettings.dspTime + _buffer > _nextTick) {
            _source[flip].PlayScheduled(_nextTick);
            _nextTick += 60.0d / _bpm;
            flip = 1 - flip;
        }
    }
}


// Audio Thread version

// [RequireComponent(typeof(AudioSource))]
// public class Metronome : MonoBehaviour
// {
//     private double _bpm = 140.0F;
//     public float gain = 0.5F;
//     public int signatureHi = 4;
//     public int signatureLo = 4;

//     private double nextTick = 0.0F;
//     private float amp = 0.0F;
//     private float phase = 0.0F;
//     private double sampleRate = 0.0F;
//     private int accent;

//     public void StartMetronome(float bpm)
//     {
//         _bpm = (double)bpm;
//         accent = signatureHi;
//         double startTick = AudioSettings.dspTime;
//         sampleRate = AudioSettings.outputSampleRate;
//         nextTick = startTick * sampleRate;
//         GetComponent<AudioSource>().Play();
//     }

//     void OnAudioFilterRead(float[] data, int channels)
//     {
//         double samplesPerTick = sampleRate * 60.0F / _bpm * 4.0F / signatureLo;
//         double sample = AudioSettings.dspTime * sampleRate;
//         int dataLen = data.Length / channels;

//         int n = 0;
//         while (n < dataLen)
//         {
//             float x = gain * amp * Mathf.Sin(phase);
//             int i = 0;
//             while (i < channels)
//             {
//                 data[n * channels + i] += x;
//                 i++;
//             }
//             while (sample + n >= nextTick)
//             {
//                 nextTick += samplesPerTick;
//                 amp = 1.0F;
//                 if (++accent > signatureHi)
//                 {
//                     accent = 1;
//                     amp *= 2.0F;
//                 }
//                 Debug.Log("Tick: " + accent + "/" + signatureHi);
//             }
//             phase += amp * 0.3F;
//             amp *= 0.993F;
//             n++;
//         }
//     }
// }