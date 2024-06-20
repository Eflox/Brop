using Brop.Views;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brop.Models
{
    public class Match : NetworkBehaviour
    {
        private List<Player> players;
        private List<GameObject> balls = new List<GameObject>();
        private float spawnAreaWidth = 10f;
        private float spawnAreaHeight = 1f;
        private Vector2 spawnAreaTopLeft = new Vector2(-6f, 3f);

        public void Initialize(List<Player> players)
        {
            this.players = players;
            Debug.Log("Match initialized with players: " + string.Join(", ", players.Select(p => p.Name)));
            StartMatch();
        }

        private void StartMatch()
        {
            Debug.Log("Match Started");
            StartCoroutine(SpawnBalls());
        }

        private IEnumerator SpawnBalls()
        {
            Debug.Log("Spawning balls");

            yield return new WaitUntil(() => SceneManager.GetSceneByName("LevelScene").isLoaded);

            GameObject ballPrefab = Resources.Load<GameObject>("BallPrefab");
            Debug.Log("BallPrefab loaded: " + (ballPrefab != null));

            foreach (Player player in players)
            {
                Vector2 spawnPosition = new Vector2(
                    Random.Range(spawnAreaTopLeft.x, spawnAreaTopLeft.x + spawnAreaWidth),
                    Random.Range(spawnAreaTopLeft.y, spawnAreaTopLeft.y + spawnAreaHeight)
                );

                // Instantiate the ball locally on the server
                GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Ball instantiated for player: " + player.Name);

                // Set the player name on the ball view
                BallView ballView = ball.GetComponent<BallView>();
                if (ballView != null)
                {
                    ballView.SetBall(player.Name);
                    Debug.Log("BallView set for player: " + player.Name);
                }
                else
                {
                    Debug.LogError("BallView component not found on ball prefab.");
                }

                // Set Rigidbody2D to be kinematic to freeze the ball
                Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                // NetworkServer.Spawn to synchronize the ball across clients
                NetworkServer.Spawn(ball);
                Debug.Log("Ball spawned on the network for player: " + player.Name);

                balls.Add(ball);
            }

            // Wait for 5 seconds before dropping the balls
            yield return new WaitForSeconds(5);

            // Enable physics on the balls
            foreach (GameObject ball in balls)
            {
                Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
            }

            Debug.Log("Balls dropped");
        }

        private void Update()
        {
            foreach (GameObject ball in balls)
            {
                if (ball.transform.position.y <= -4.8f)
                {
                    DeclareWinner(ball);
                    break;
                }
            }
        }

        private void DeclareWinner(GameObject winningBall)
        {
            Player winner = players[balls.IndexOf(winningBall)];
            Debug.Log(winner.Name + " is the winner!");

            foreach (Player player in players)
            {
                // Notify players about the winner
                TargetNotifyWinner(player.Connection, winner.Name);
            }

            foreach (GameObject ball in balls)
            {
                Destroy(ball.GetComponent<Rigidbody2D>());
                //NetworkServer.Destroy(ball);
            }

            balls.Clear();

            //StartCoroutine(ReturnToMenuScene());
        }

        private IEnumerator ReturnToMenuScene()
        {
            yield return new WaitForSeconds(3f);

            // Unload the current level scene
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("LevelScene");
            while (!unloadOp.isDone)
            {
                yield return null;
            }

            // Notify clients to switch to MenuScene
            NetworkManager.singleton.ServerChangeScene("MenuScene");
        }

        [TargetRpc]
        private void TargetNotifyWinner(NetworkConnection target, string winnerName)
        {
            Debug.Log("Winner is: " + winnerName);
            // You can add additional code here to notify the client, like displaying a UI message
        }
    }
}