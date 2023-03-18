using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
public class RhythmLevelGenerator : MonoBehaviour
{
    public float rms; // avg power output
    public float db; // volume
    public float pitch;

    private AudioSource _source;
    private float[] _samples;
    private float[] _spectrum = new float[Constants.SAMPLE_SIZE];
    private float _sampleRate;

    private void Start()
    {
        // _source = GetComponent<AudioSource>();
        // _sampleRate = AudioSettings.outputSampleRate;
    }

    public void AnalyzeWave(AudioClip clip)
    {
        _samples = new float[clip.samples * clip.channels];
        clip.GetData(_samples, 0);
        
    }

    private void Update()
    {
        _source.GetOutputData(_samples, 0);

        float sumOfSquare = _samples.Sum(s => s * s);
        rms = Mathf.Sqrt(sumOfSquare / Constants.SAMPLE_SIZE);
        db = 20 * Mathf.Log10(rms / 0.1f);

        _source.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);

        float maxSpectrum = _spectrum.Max();
        int maxSpectrumIndex = Array.IndexOf(_spectrum, maxSpectrum);

        float freq = maxSpectrumIndex;
        if (maxSpectrumIndex > 0 && maxSpectrumIndex < Constants.SAMPLE_SIZE - 1)
        {
            float dL = _spectrum[maxSpectrumIndex - 1] / _spectrum[maxSpectrumIndex];
            float dR = _spectrum[maxSpectrumIndex + 1] / _spectrum[maxSpectrumIndex];
            freq += 0.5f * (dR * dR - dL * dL);
        }
        pitch = freq * (_sampleRate / 2) / Constants.SAMPLE_SIZE;
    }
}

}
