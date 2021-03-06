﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[NetworkSettings(sendInterval = 0.01f)]
public class LeapPlayer : NetworkBehaviour
{
    public void Start()
    {
        Debug.Log("Leap Player Start");
        Application.targetFrameRate = 20;
    }

    public LeapHand leftHand;
    public LeapHand rightHand;

    public LeapAudio leapAudio;

    public void BeginHand(int hand, byte[] arrHand)
    {
        byte[] newHand = CLZF.Compress(arrHand);

        Debug.Log("Begin Hand:" + hand + " from " + arrHand.Length + " to " + newHand.Length);

        CmdBeginHand(hand, newHand);
    }

    public void FinishHand(int hand)
    {
        Debug.Log("Finish Hand:" + hand);

        CmdFinishHand(hand);
    }

    public void SetLeapHand(int hand, byte[] arrHand)
    {
        byte[] newHand = CLZF.Compress(arrHand);

        CmdSetLeapHand(hand, newHand);
    }

    public void SendAudio(float[] f, int chan)
    {
        byte[] newBytes = CLZF.CompressAudio(f);

        //Debug.Log("Audio Length:" + newBytes.Length);

        CmdAudioSend(newBytes, chan);
    }

    [Command(channel = 0)]
    public void CmdBeginHand(int hand, byte[] arrHand) { }

    [Command(channel = 0)]
    public void CmdFinishHand(int hand) { }

    [Command(channel = 1)]
    public void CmdSetLeapHand(int hand, byte[] arrHand) { }

    [Command(channel = 2)]
    public void CmdAudioSend(byte[] f, int chan) { }
}