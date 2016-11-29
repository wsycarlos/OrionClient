using UnityEngine;
using System.Collections;

public class LeapAudio : MonoBehaviour
{
    LeapPlayer _player = null;
    private LeapPlayer player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<LeapPlayer>();
            }
            return _player;
        }
    }

    float delay = 1f;
    float last = -1f;

    int lastSample;
    AudioClip c;
    
    private const int FREQUENCY = 11025;

    void Start()
    {
        c = Microphone.Start(null, true, 20000, FREQUENCY);
        while (Microphone.GetPosition(null) < 0) { } // HACK from Riro
    }

    void Update()
    {
        if (last + delay < Time.realtimeSinceStartup)
        {
            int pos = Microphone.GetPosition(null);
            int diff = pos - lastSample;
            if (diff > 0)
            {
                float[] samples = new float[diff * c.channels];
                c.GetData(samples, lastSample);
                if (player != null)
                {
                    player.SendAudio(samples, c.channels);
                }
            }
            lastSample = pos;
            last = Time.realtimeSinceStartup;
        }
    }

    
    public void Set(float[] f, int chan)
    {
        
    }
}