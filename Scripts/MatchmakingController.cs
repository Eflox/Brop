/*
 * MatchmakingController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 19/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using Brop.Models;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Brop
{
    public class MatchmakingController : NetworkBehaviour
    {
        [SerializeField]
        private UIController _ui;

        public static MatchmakingController Instance;

        [SerializeField]
        private List<Player> _playersInQueue = new List<Player>();

        private int _requiredPlayerCount = 5;

        private List<Player> _matchPlayers = new List<Player>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        [Server]
        public void AddPlayerToQueue(NetworkIdentity networkIdentity)
        {
            var playerToAdd = PlayersManager.Instance.GetPlayer(networkIdentity.connectionToClient);

            if (_playersInQueue.Contains(playerToAdd))
            {
                Debug.Log($"{playerToAdd.Name} ALREADY IN QUEUE");
                return;
            }

            _playersInQueue.Add(playerToAdd);

            Debug.Log($"{playerToAdd.Name} JOINED QUEUE");
            Debug.Log($"({_playersInQueue.Count}/{_requiredPlayerCount}) TO START A MATCH");
            RpcUpdateQueueCount(_playersInQueue.Count);

            if (_playersInQueue.Count == _requiredPlayerCount)
                CreateMatch();
        }

        [Server]
        public void RemovePlayerFromQueue(NetworkConnectionToClient networkConnection)
        {
            var playerToRemove = _playersInQueue.Find(player => player.NetworkIdentity.connectionToClient == networkConnection);

            if (playerToRemove == null)
                return;

            _playersInQueue.Remove(playerToRemove);

            Debug.Log($"{playerToRemove.Name} LEFT QUEUE");
            Debug.Log($"({_playersInQueue.Count}/{_requiredPlayerCount}) TO START A MATCH");

            RpcUpdateQueueCount(_playersInQueue.Count);
        }

        [ClientRpc]
        private void RpcUpdateQueueCount(int playerCount)
        {
            _ui.UpdatePlayerInQeueCount(playerCount);
        }

        private void CreateMatch()
        {
            Debug.Log("START MATCH");
            GameObject matchSimulator = new GameObject("---MATCHSIMULATOR---");

            _matchPlayers = new List<Player>(_playersInQueue);
            matchSimulator.AddComponent<SimulationRecorder>().Initialize(_matchPlayers);

            foreach (var player in _matchPlayers)
            {
                RemovePlayerFromQueue(player.NetworkIdentity.connectionToClient);
                TargetLoadMatchUI(player.NetworkIdentity.connectionToClient);
            }
        }

        [TargetRpc]
        private void TargetLoadMatchUI(NetworkConnection networkConnection)
        {
            _ui.LoadMatch();
        }

        public void GetSimulation(MatchRecording matchRecording)
        {
            foreach (var player in _matchPlayers)
                TargetLoadMatchForPlayers(player.NetworkIdentity.connectionToClient, matchRecording);

            //_matchPlayers.Clear();

            Debug.Log($"{matchRecording.Winner} WINS!");
        }

        public void MatchFinished(string winner)
        {
            _ui.ShowWinner(winner);
        }

        [TargetRpc]
        private void TargetLoadMatchForPlayers(NetworkConnection networkConnection, MatchRecording matchRecording)
        {
            _ui.CloseAll();
            GameObject matchSimulator = new GameObject("---MATCHSIMULATOR---");
            var playback = matchSimulator.AddComponent<SimulationPlayer>();

            playback.StartPlayback(matchRecording);
        }
    }
}