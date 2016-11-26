using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace LeapNet
{
    public class LeapNet : NetworkManager
    {
        public bool isDebug = false;

        void Start()
        {
            if (isDebug)
            {
                networkAddress = "127.0.0.1";
                StartClient();
            }
            else
            {
                networkAddress = "192.168.137.184";
                StartClient();
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            Debug.Log("Server Connected!");
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            Debug.Log("Server Disconnected!");
        }
    }
}