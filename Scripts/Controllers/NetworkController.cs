/*
 * NetworkController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 14/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using Mirror;
using UnityEngine;

namespace Brop.Controllers
{
    public class NetworkController : NetworkManager
    {
        public GameObject ballPrefab;

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<ConnectMessage>(OnServerConnectMessage);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            NetworkClient.RegisterHandler<ConnectMessage>(OnClientConnectMessage);
            // Load the prefab from the Resources folder
            GameObject ballPrefab = Resources.Load<GameObject>("BallPrefab");

            if (ballPrefab != null)
            {
                // Register the prefab with the NetworkClient
                NetworkClient.RegisterPrefab(ballPrefab);
                Debug.Log("BallPrefab registered successfully.");
            }
            else
            {
                Debug.LogError("BallPrefab not found in Resources.");
            }
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            // Send ConnectMessage to the server
            ConnectMessage connectMessage = new ConnectMessage
            {
                playerName = UIController.PlayerName // Use the static variable
            };

            NetworkClient.Send(connectMessage);
        }

        private void OnClientConnectMessage(ConnectMessage msg)
        {
            // Handle the ConnectMessage on the client side if needed
            Debug.Log($"Connected as {msg.playerName}");
        }

        private void OnServerConnectMessage(NetworkConnection conn, ConnectMessage msg)
        {
            // Handle the ConnectMessage on the server side
            Debug.Log($"Server received connection message from {msg.playerName}");
            MatchmakingServerController.Instance.SearchForMatch(msg.playerName, conn);
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("Player added to server");
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName == "LevelScene")
            {
                Debug.Log("Server scene changed to LevelScene");
            }
        }
    }
}