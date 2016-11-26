using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LeapPlayer : NetworkBehaviour
{
    public float delay = 0.1f;

    public LeapHand leftHand;
    public LeapHand rightHand;

    private float leftLastTime = -1f;
    private float rightLastTime = -1f;

    public void FinishHand(int hand)
    {
        CmdFinishHand(hand);
    }

    public void SetLeapHand(int hand, byte[] arrHand)
    {
        if(hand ==0)
        {
            if (leftLastTime + delay < Time.realtimeSinceStartup)
            {
                CmdSetLeapHand(hand, arrHand);
                leftLastTime = Time.realtimeSinceStartup;
            }
        }
        else if(hand == 1)
        {
            if (rightLastTime + delay < Time.realtimeSinceStartup)
            {
                CmdSetLeapHand(hand, arrHand);
                rightLastTime = Time.realtimeSinceStartup;
            }
        }
    }
    
    [Command]
    public void CmdFinishHand(int hand)
    {
        if (hand == 0)
        {
            leftHand.FinishHand();
        }
        else
        {
            rightHand.FinishHand();
        }
    }

    [Command]
    public void CmdSetLeapHand(int hand, byte[] arrHand)
    {        
        if (hand == 0)
        {
            leftHand.SetLeapHand(arrHand);
        }
        else
        {
            rightHand.SetLeapHand(arrHand);
        }
    }

}