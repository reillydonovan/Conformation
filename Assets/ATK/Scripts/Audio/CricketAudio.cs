using UnityEngine;
using ATKSharp.Generators.Oscillators.Wavetable;
using ATKSharp.Envelopes;
using System.Collections;
using ATKSharp.Generators.Oscillators.Trivial;

[RequireComponent(typeof(AudioSource))]
public class CricketAudio : MonoBehaviour
{

    [Header("Chirp")]
    [Range(0f, 20000f)]
    [SerializeField]
    float centerFrequency = 3259f;
    [SerializeField]
    float frequencySpread = .07f;
    [SerializeField]
    float chirpDuration = .140f;
    [SerializeField]
    float chirpGap = .020f;
    [Range(5f,10f)]
    [SerializeField]
    float chirpPause = 5f;
    [Range(0f, 1f)]
    [SerializeField]
    float chirpPauseChance = .1f;
    [SerializeField]
    int chirpPulses = 4;
    [SerializeField]
    float chirpsPerSecond = 2;
    [Range(0f, 1f)]
    [SerializeField]
    float chirpAmplitude = 1f;

    float currentFrequency;

    TPhasor control;
    WTSine fundamental, octave, modulator;
    CTEnvelope chirpEnvelope;

    private void Awake()
    {
        Application.runInBackground = true;
    }

    private void Start()
    {
        control = new TPhasor(1 / chirpDuration);
        fundamental = new WTSine(centerFrequency);
        octave = new WTSine(centerFrequency * .5f);
        modulator = new WTSine(centerFrequency * 2f);
        chirpEnvelope = new CTEnvelope(20,0,.7f,130);
        StartCoroutine(Chirp());
    }

    private void Update()
    {
        fundamental.Frequency = centerFrequency;
    }

    IEnumerator Chirp()
    {
        for (int i = 0; i < chirpPulses; i++)
        {
            chirpEnvelope.Gate = 1;
            yield return new WaitForSeconds(chirpDuration);
            chirpEnvelope.Gate = 0;
            yield return new WaitForSeconds(chirpGap);
        }
        yield return new WaitForSeconds(1 / chirpsPerSecond);
        if (Random.value < chirpPauseChance)
        {
            yield return new WaitForSeconds(chirpPause);
        }
        StartCoroutine(Chirp());
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            EnvelopeState lastEnvelopeState = chirpEnvelope.State;
            if (chirpEnvelope.State!=lastEnvelopeState && chirpEnvelope.State == EnvelopeState.ATTACK)
            {
                control.Phase = 0;
            }
            float controlFrequency = 1 - (control.Generate() * frequencySpread);
            fundamental.Frequency = centerFrequency * controlFrequency;
            octave.Frequency = fundamental.Frequency * 2;
            octave.Amplitude = .3f;
            modulator.Frequency = controlFrequency * 40.6f;
            modulator.Generate();
            float modulatorSquared = modulator.CurrentSample * modulator.CurrentSample;
            float rawSample = modulatorSquared * (fundamental.Generate()+octave.Generate());
            float currentSample = chirpEnvelope.Generate() * rawSample * chirpAmplitude;
            for (int j = 0; j < channels; j++)
            {
                data[i + j] = currentSample; //apply it to each channel
            }
        }
    }
}
