/*
 * MatchmakingServerController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 19/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using Brop.Models;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brop
{
    public class MatchmakingServerController : NetworkBehaviour
    {
        public static MatchmakingServerController Instance;
        private List<Player> searchingPlayers = new List<Player>();
        private int requiredPlayerCount = 5;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SearchForMatch(string playerName, NetworkConnection conn)
        {
            Debug.Log($"Searching for match for player: {playerName}");

            if (IsValidPlayerName(playerName))
            {
                Player newPlayer = new Player(playerName, conn);
                searchingPlayers.Add(newPlayer);
                Debug.Log($"Player {playerName} added to the queue. Queue size: {searchingPlayers.Count}");

                TargetLogQueueMessage(conn, "You have been added to the matchmaking queue.");

                if (searchingPlayers.Count >= requiredPlayerCount)
                {
                    Debug.Log("Required player count reached. Creating match...");
                    CreateMatch(searchingPlayers.GetRange(0, requiredPlayerCount));
                    searchingPlayers.RemoveRange(0, requiredPlayerCount);
                }
            }
            else
            {
                Debug.Log($"Invalid player name: {playerName}");
                conn.Disconnect();
            }
        }

        private bool IsValidPlayerName(string playerName)
        {
            return !string.IsNullOrEmpty(playerName) && playerName.Length <= 20;
        }

        private void CreateMatch(List<Player> players)
        {
            Debug.Log("Creating match with players: " + string.Join(", ", players.Select(p => p.Name)));
            string matchSceneName = "LevelScene";
            SceneManager.LoadScene(matchSceneName, LoadSceneMode.Additive);
            StartCoroutine(InitializeMatchAfterSceneLoad(players, matchSceneName));
        }

        private IEnumerator InitializeMatchAfterSceneLoad(List<Player> players, string matchSceneName)
        {
            yield return new WaitUntil(() => SceneManager.GetSceneByName(matchSceneName).isLoaded);

            Scene matchScene = SceneManager.GetSceneByName(matchSceneName);

            // Notify all clients to load the match scene
            foreach (var player in players)
            {
                TargetLoadMatchScene(player.Connection, matchSceneName);
            }

            // Create and initialize the match
            GameObject matchObject = new GameObject("Match");
            Match newMatch = matchObject.AddComponent<Match>();
            newMatch.Initialize(players);

            // Move players' GameObjects to the match scene
            foreach (Player player in players)
            {
                SceneManager.MoveGameObjectToScene(player.Connection.identity.gameObject, matchScene);
            }

            // Move the Match object to the match scene
            SceneManager.MoveGameObjectToScene(matchObject, matchScene);
        }

        [TargetRpc]
        private void TargetLoadMatchScene(NetworkConnection target, string matchSceneName)
        {
            Debug.Log("Client: Loading match scene " + matchSceneName);
            SceneManager.LoadScene(matchSceneName, LoadSceneMode.Additive);
        }

        [TargetRpc]
        private void TargetLogQueueMessage(NetworkConnection target, string message)
        {
            Debug.Log(message);
        }
    }
}