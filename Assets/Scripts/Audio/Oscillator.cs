using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

    public double frequency = 440.0;
    private double increment;
    private double phase;
    private double samplingFrequency = 48000.0;
    public float gain;

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / samplingFrequency;

        for(int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase));

            if (channels == 2) data[i + 1] = data[i];
            if (phase > Mathf.PI * 2) phase = 0.0f;

        }
    }

    private void Update()
    {
        frequency = Mathf.PingPong(Time.time * 200.0f, 20000.0f) + 100.0f;

    }
}
