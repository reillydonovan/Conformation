using UnityEngine;
using ATKSharp.Generators;
using ATKSharp.Generators.Noise;
using ATKSharp.Modifiers;
using ATKSharp;
using System;

[RequireComponent(typeof(AudioSource))]
public class FireAudio : MonoBehaviour
{

    class FireCracklePop
    {
        public double duration = 1f;
        public double decrementValue = 1;
        public float amplitude = 1f;
        public Butterworth butterworth;
        public double envelopeValue;

        public FireCracklePop()
        {
            butterworth = new Butterworth();
        }
    }

    [Header("Base Fire Noise")]
    [Range(0f, 20000f)]
    [SerializeField]
    float baseNoiseLowCutoff = 100f;
    [Range(0f, 20000f)]
    [SerializeField]
    float baseNoiseHighCutoff = 3000f;
    [Range(0f, 1f)]
    [SerializeField]
    float baseNoiseAmplitude = .05f;
    [Header("Crackle")]
    [SerializeField]
    float minCrackleDuration = 2f;
    [SerializeField]
    float maxCrackleDuration = 12f;
    [Range(.1f,20f)]
    [SerializeField]
    float crackleResonance = 12f;
    [Header("Pop")]
    [SerializeField]
    float minPopDuration = 2f;
    [SerializeField]
    float maxPopDuration = 12f;

    //Base Fire Noise
    WhiteNoise baseNoise;
    LowPass baseNoiseLowPass;
    HighPass baseNoiseHighPass;
    //Crackle
    FireCracklePop crackle;
    ImpulseGenerator crackleTrigger;
    float previousCrackleTriggerState = 0f;
    //Gas leak
    WhiteNoise gasNoise;
    LowPass gasLowPass1, gasLowPass2;
    HighPass gasHighPass;
    [Range(0f,1f)]
    [SerializeField]
    float gasAmplitude = .001f;
    Biquad gasLowBiquad1,gasLowBiquad2,gasHighBiquad;
    
    System.Random rand;

    private void Awake()
    {
        Application.runInBackground = true;
    }

    private void Start()
    {
        baseNoise = new WhiteNoise();
        baseNoiseLowPass = new LowPass(baseNoiseLowCutoff);
        baseNoiseHighPass = new HighPass(baseNoiseHighCutoff);
        crackle = new FireCracklePop();
        crackleTrigger = new ImpulseGenerator(60f);
        crackleTrigger.PulseDeviation = 1f;
        crackleTrigger.BurstMasking = .8f;
        gasNoise = new WhiteNoise();
        gasLowPass1 = new LowPass(1f);
        gasLowBiquad2 = new Biquad(ModifierType.LowPass, 2500f, 1f, 1f);
        gasHighBiquad = new Biquad(ModifierType.HighPass, 1000f, 1f, 1f);
        rand = new System.Random();
    }

    private void Update()
    {
        baseNoiseLowPass.Frequency = baseNoiseLowCutoff;
        baseNoiseHighPass.Frequency = baseNoiseHighCutoff;
        crackle.butterworth.Bandwidth = crackleResonance;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            float currentSample = baseNoise.Generate() * baseNoiseAmplitude;
            float lowPassBaseNoise = baseNoiseLowPass.Modify(currentSample);
            float highPassBaseNoise = baseNoiseHighPass.Modify(currentSample);
            if (crackleTrigger.Generate() == 1 && previousCrackleTriggerState == 0)
            {

                float durationMS = (float)rand.NextDouble() * (maxCrackleDuration-minCrackleDuration) + minCrackleDuration;
                crackle.duration = durationMS * 0.001f * ATKSettings.SampleRate;
                crackle.amplitude = (float)rand.NextDouble() * .3f;
                crackle.butterworth.Frequency = durationMS * 1000 + 2500;
                crackle.decrementValue = 1 / crackle.duration;
                crackle.envelopeValue = 1f;
            }
            previousCrackleTriggerState = crackleTrigger.CurrentSample;
            currentSample += lowPassBaseNoise + highPassBaseNoise;
            crackle.envelopeValue -= crackle.decrementValue;
            if (crackle.envelopeValue <= 0)
            {
                crackle.envelopeValue = 0;
            }
            float crackleSample = crackle.butterworth.Modify(baseNoise.CurrentSample) * (float)(crackle.envelopeValue*crackle.envelopeValue) * crackle.amplitude;
            currentSample += crackleSample;
            //Gas
            float gasNoiseSample = gasNoise.Generate();
            float gasLowPassNoise = gasLowPass1.Modify(gasNoiseSample) * 10;
            gasLowPassNoise = gasLowPassNoise * gasLowPassNoise;
            gasLowPassNoise = gasLowPassNoise * gasLowPassNoise;
            gasLowPassNoise *= 600;
            float filteredGasNoise = gasHighBiquad.Modify(gasNoiseSample) * gasLowPassNoise;
            float gasSample = gasLowBiquad2.Modify(filteredGasNoise);
            currentSample += gasSample *gasAmplitude;
            //End Gas
            for (int j = 0; j < channels; j++)
            {
                data[i + j] = currentSample; //apply it to each channel
            }
        }
    }
}
