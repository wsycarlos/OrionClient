using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[NetworkSettings(sendInterval = 0.01f)]
public class LeapPlayer : NetworkBehaviour
{
    public void Start()
    {
        Debug.Log("Leap Player Start");
        Application.targetFrameRate = 100;
    }

    public LeapHand leftHand;
    public LeapHand rightHand;

    public LeapAudio leapAudio;

    public void BeginHand(int hand, byte[] arrHand)
    {
        Debug.Log("Begin Hand:" + hand + " with " + arrHand.Length);

        CmdBeginHand(hand, arrHand);
    }

    public void FinishHand(int hand)
    {
        Debug.Log("Finish Hand:" + hand);

        CmdFinishHand(hand);
    }

    public void SetLeapHand(int hand, byte[] arrHand)
    {
        Debug.Log("Set Hand:" + hand + " with " + arrHand.Length);

        CmdSetLeapHand(hand, arrHand);
    }

    public void SendAudio(float[] f, int chan)
    {
        Debug.Log("Set Audio:" + chan + " with " + f.Length);

        CmdAudioSend(f, chan);
    }

    [Command(channel = 0)]
    public void CmdBeginHand(int hand, byte[] arrHand) { }

    [Command(channel = 0)]
    public void CmdFinishHand(int hand) { }

    [Command(channel = 1)]
    public void CmdSetLeapHand(int hand, byte[] arrHand) { }

    [Command(channel = 2)]
    public void CmdAudioSend(float[] f, int chan) { }
}