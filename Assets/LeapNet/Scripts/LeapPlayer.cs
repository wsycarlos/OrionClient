using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LeapPlayer : NetworkBehaviour
{
    public float delay = 0.1f;

    void Start()
    {
        Debug.Log("Leap Player Start");
        Debug.Log(transform.position);
    }

    void Update()
    {
        if (lastLeftFinish)
        {
            CmdFinishHand(0);
            lastLeftFinish = false;
        }
        if (lastRightFinish)
        {
            CmdFinishHand(1);
            lastRightFinish = false;
        }
        if (lastLeftSet)
        {
            CmdSetLeapHand(0, lastLeftSetArr);
            lastLeftSet = false;
            lastLeftSetArr = null;
        }
        if (lastRightSet)
        {
            CmdSetLeapHand(1, lastRightSetArr);
            lastRightSet = false;
            lastRightSetArr = null;
        }
    }

    public LeapHand leftHand;
    public LeapHand rightHand;

    private float leftLastTime = -1f;
    private float rightLastTime = -1f;

    private bool lastLeftFinish = false;
    private bool lastRightFinish = false;

    private bool lastLeftSet = false;
    private byte[] lastLeftSetArr = null;
    private bool lastRightSet = false;
    private byte[] lastRightSetArr = null;

    public void FinishHand(int hand)
    {
        if (hand == 0)
        {
            lastLeftFinish = true;
        }
        else
        {
            lastRightFinish = true;
        }
    }

    public void SetLeapHand(int hand, byte[] arrHand)
    {
        if(hand ==0)
        {
            if (leftLastTime + delay < Time.realtimeSinceStartup)
            {
                lastLeftSet = true;
                lastLeftSetArr = arrHand;
                leftLastTime = Time.realtimeSinceStartup;
            }
        }
        else if(hand == 1)
        {
            if (rightLastTime + delay < Time.realtimeSinceStartup)
            {
                lastRightSet = true;
                lastRightSetArr = arrHand;
                rightLastTime = Time.realtimeSinceStartup;
            }
        }
    }
    
    [Command]
    public void CmdFinishHand(int hand)
    {
        Debug.Log("Finish Hand" + hand);
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
        Debug.Log("Set Hand" + hand + " with " + arrHand.Length);
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