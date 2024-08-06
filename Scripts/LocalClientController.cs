/*
 * LocalClientController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 26/07/2024
 * Contact: c.dansembourg@icloud.com
 */

using Mirror;
using UnityEngine;

namespace Brop
{
    public class LocalClientController : MonoBehaviour
    {
        [SerializeField]
        private UIController _UIController;

        public ConnectClientController ConnectClientController;

        public void SetName(string name)
        {
            _UIController.SetName(name);
        }

        public void JoinQueue()
        {
            ConnectClientController.JoinQueue();
        }

        public void LeaveQueue()
        {
            ConnectClientController.LeaveQueue();
        }

        public void ConnectHost()
        {
            if (ConnectionChecks())
            {
                Debug.Log("STARTING SERVER...");
                NetworkManager.singleton.StartServer();
            }
        }

        public void DisconnectHost()
        {
            if (!ConnectionChecks())
            {
                Debug.Log("STOPPING SERVER...");
                NetworkManager.singleton.StopServer();
            }
        }

        public void ConnectClient()
        {
            if (ConnectionChecks())
            {
                Debug.Log("CONNECTING CLIENT...");
                NetworkManager.singleton.StartClient();
            }
        }

        public void DisconnectClient()
        {
            if (!ConnectionChecks())
            {
                Debug.Log("DISCONNECTING CLIENT...");
                NetworkManager.singleton.StopClient();
            }
        }

        private bool ConnectionChecks()
        {
            if (NetworkServer.active)
                return false;

            if (NetworkClient.isConnected)
                return false;

            return true;
        }
    }
}