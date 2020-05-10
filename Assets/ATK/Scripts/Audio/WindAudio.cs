using UnityEngine;
using ATKSharp.Generators.Noise;
using ATKSharp.Modifiers;

[RequireComponent(typeof(AudioSource))]
public class WindAudio : MonoBehaviour
{
    [Header("Base Wind Noise")]
    [Range(0f, 20000f)]
    [SerializeField]
    float baseNoiseLowCutoff = 100f;
    [Range(0f, 1f)]
    [SerializeField]
    float baseNoiseAmplitude = .05f;

    //Base Fire Noise
    WhiteNoise baseNoise;
    LowPass baseNoiseLowPass;

    private void Awake()
    {
        Application.runInBackground = true;
    }

    private void Start()
    {
        baseNoise = new WhiteNoise();
        baseNoiseLowPass = new LowPass(baseNoiseLowCutoff);
    }

    private void Update()
    {
        baseNoiseLowPass.Frequency = baseNoiseLowCutoff;
        baseNoiseAmplitude = .001f + Mathf.PingPong(Time.time * .007f, .01f) + Mathf.PingPong(Time.time*.0022f,.01f);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            float currentSample = baseNoise.Generate() * baseNoiseAmplitude;
            float lowPassBaseNoise = baseNoiseLowPass.Modify(currentSample);
            currentSample += lowPassBaseNoise;
            for (int j = 0; j < channels; j++)
            {
                data[i + j] = currentSample; //apply it to each channel
            }
        }
    }
}
