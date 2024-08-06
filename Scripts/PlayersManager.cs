using Brop.Models;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Brop
{
    public class PlayersManager : NetworkBehaviour
    {
        [SerializeField]
        private UIController _ui;

        [SerializeField]
        private List<Player> _connectedPlayers = new List<Player>();

        [SerializeField]
        private List<Player> _logggedInPlayers = new List<Player>();

        public static PlayersManager Instance { get; private set; }

        public Player GetPlayer(NetworkConnection conn)
        {
            return _connectedPlayers.Find(player => player.NetworkIdentity.connectionToClient == conn);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        [Server]
        public void CreatePlayer(Player player)
        {
            if (!isServer)
                return;

            _connectedPlayers.Add(player);
            Debug.Log($"{player.Name} ADDED TO CONNECTED PLAYERS LIST");

            RpcUpdatePlayerCount(_connectedPlayers.Count);
        }

        [Server]
        public void RemovePlayer(NetworkConnectionToClient conn)
        {
            if (!isServer)
                return;

            var playerToRemove = _connectedPlayers.Find(player => player.NetworkIdentity.connectionToClient == conn);
            _connectedPlayers.Remove(playerToRemove);
            Debug.Log($"{playerToRemove.Name} REMOVED FROM CONNECTED PLAYERS LIST");

            RpcUpdatePlayerCount(_connectedPlayers.Count);
        }

        [ClientRpc]
        private void RpcUpdatePlayerCount(int playerCount)
        {
            _ui.UpdatePlayerCount(playerCount);
        }
    }
}