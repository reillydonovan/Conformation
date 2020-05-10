using UnityEngine;
using ATKSharp.Generators.Oscillators.Wavetable;
using ATKSharp.Envelopes;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ChimeAudio : MonoBehaviour
{

    [Header("Chime")]
    [Range(0f, 20000f)]
    [SerializeField]
    float chimeHz = 528f;
    [Range(0f, 1f)]
    [SerializeField]
    float chimeAmplitude = 1f;

    float currentFrequency;

    WTSine chimeGenerator;
    CTEnvelope chimeEnvelope;

    private void Awake()
    {
        Application.runInBackground = true;
    }

    private void Start()
    {
        chimeGenerator = new WTSine(chimeHz);
        chimeEnvelope = new CTEnvelope(20,5,.7f,3000);
    }

    private void Update()
    {
        chimeGenerator.Frequency = chimeHz;
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Chime());
        chimeAmplitude = Mathf.Clamp01(collision.relativeVelocity.magnitude) * .7f;
    }

    IEnumerator Chime()
    {
        chimeEnvelope.Gate = 1;
        yield return new WaitForSeconds(.1f);
        chimeEnvelope.Gate = 0;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            float currentSample = chimeEnvelope.Generate() * chimeGenerator.Generate() * chimeAmplitude;
            for (int j = 0; j < channels; j++)
            {
                data[i + j] = currentSample; //apply it to each channel
            }
        }
    }
}
