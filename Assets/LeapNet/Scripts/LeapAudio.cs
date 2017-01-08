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

    int lengthSec = 1;
    
    AudioClip c;
    
    private const int FREQUENCY = 22050;

    void Start()
    {
        c = Microphone.Start(null, false, lengthSec, FREQUENCY);
        //while (Microphone.GetPosition(null) < 0) { } // HACK from Riro
    }

    void Update()
    {
        if (!Microphone.IsRecording(null))
        {
            if (c.samples > 0)
            {
                float[] samples = new float[c.samples * c.channels];
                c.GetData(samples, 0);
                if (player != null)
                {
                    player.SendAudio(samples, c.channels);
                }
            }
            c = Microphone.Start(null, false, lengthSec, FREQUENCY);
        }
    }

    
    public void Set(float[] f, int chan)
    {
        
    }
}