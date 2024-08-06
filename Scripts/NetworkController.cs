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
        [SerializeField]
        private GameObject _ballPrefab;

        [SerializeField]
        private UIController _UIController;

        [SerializeField]
        private LocalClientController _localClientController;

        //SERVER-SIDE

        /// <summary>
        /// Server-side function that is called when the server is started
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();

            _UIController.ServerConnected();

            Debug.Log("SERVER STARTED");
        }

        /// <summary>
        /// Server-side function that is called when the server is stopped
        /// </summary>
        public override void OnStopServer()
        {
            base.OnStopServer();

            _UIController.ServerDisconnected();
            Debug.Log("SERVER STOPPED");
        }

        /// <summary>
        /// Server-side function that is called when a client connects to the server
        /// </summary>
        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);

            Debug.Log($"[{conn}] CONNECTED TO SERVER");
        }

        /// <summary>
        /// Server-side function that is called when a client disconnects from the server
        /// </summary>
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);

            PlayersManager.Instance.RemovePlayer(conn);
            MatchmakingController.Instance.RemovePlayerFromQueue(conn);

            Debug.Log($"[{conn}] DISCONNECTED FROM SERVER");
        }

        //CLIENT-SIDE

        /// <summary>
        /// Client-side function that is run on the client when it is connecting to the server
        /// </summary>
        public override void OnStartClient()
        {
            base.OnStartClient();
        }

        /// <summary>
        /// Client-side function that is run on the client when it has finished connecting to the server
        /// </summary>
        public override void OnClientConnect()
        {
            base.OnClientConnect();

            _UIController.ClientConnected();
        }

        /// <summary>
        /// Client-side function that is run on the client when it has disconnected from the server
        /// </summary>
        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();

            _UIController.ClientDisconnected();
            Debug.Log("CLIENT DISCONNECTED");
        }
    }
}