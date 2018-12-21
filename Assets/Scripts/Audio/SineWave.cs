using UnityEngine;

public class SineWave : MonoBehaviour
{
    [Range(1, 500)]  //Creates a slider in the inspector
    public float frequency1 = 200;

    public float offset = 2;

    [Range(1, 500)]  //Creates a slider in the inspector
    public float frequency2 = 200;

    public float sampleRate = 44100;
    public float waveLengthInSeconds = 10.0f;
    public float masterFrequency = 1;
    public bool enableFreqLoop = false;

    AudioSource audioSource;
    int timeIndex = 0;

    float accum = 0;
    bool direction = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1; //force 2D sound
        //audioSource.Stop(); //avoids audiosource from starting to play automatically
    }

    private void Update()
    {
        if (enableFreqLoop)
        {
            accum += Time.time;

            if (frequency1 >= 500)
                direction = false;
            if (frequency1 <= 1)
                direction = true;

            if (accum >= masterFrequency)
            {
                if (direction)
                    frequency1 += 1;
                if (!direction)
                    frequency1 -= 1;
                accum = 0;
            }
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        frequency2 = frequency1 + offset;

        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(timeIndex, frequency1, sampleRate);

            if (channels == 2)
                data[i + 1] = CreateSine(timeIndex, frequency2, sampleRate);

            timeIndex++;

            //if timeIndex gets too big, reset it to 0
            if (timeIndex >= (sampleRate * waveLengthInSeconds))
            {
                timeIndex = 0;
            }
        }
    }

    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }
}