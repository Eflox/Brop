/*
 * SimulationRecorder.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 31/07/2024
 * Contact: c.dansembourg@icloud.com
 */

using Brop.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brop
{
    public class SimulationRecorder : MonoBehaviour
    {
        private List<GameObject> _balls = new List<GameObject>();
        //private List<List<Vector2>> _ballRecordings = new List<List<Vector2>>();

        private float _spawnAreaWidth = 10f;
        private float _spawnAreaHeight = 1f;
        private Vector2 _spawnAreaTopLeft = new Vector2(-6f, 3f);

        private bool _foundWinner = false;

        private MatchmakingController _matchmakingController;

        private GameObject _map;
        private MatchRecording _matchRecording;

        public void Initialize(List<Player> players)
        {
            _matchRecording = new MatchRecording();
            _matchRecording.Map = "Map1";

            Time.timeScale = 1f;

            foreach (var player in players)
            {
                _matchRecording.BallRecordings.Add(new BallRecording());
                _matchRecording.BallRecordings.Last().Name = player.Name;
            }

            _map = Instantiate(Resources.Load<GameObject>(_matchRecording.Map));
            SpawnBalls();
        }

        public void SpawnBalls()
        {
            foreach (var ball in _matchRecording.BallRecordings)
            {
                Vector2 spawnPosition = new Vector2(
                    Random.Range(_spawnAreaTopLeft.x, _spawnAreaTopLeft.x + _spawnAreaWidth),
                    Random.Range(_spawnAreaTopLeft.y, _spawnAreaTopLeft.y + _spawnAreaHeight)
                );

                var spawnedBall = Instantiate(Resources.Load<GameObject>("RecordingBall"), spawnPosition, Quaternion.identity);
                _balls.Add(spawnedBall);
            }
        }

        private void FixedUpdate()
        {
            if (_foundWinner)
            {
                DestroyAllBalls();
                return;
            }

            CheckForWinner();
            RecordBallPositions();
        }

        private void CheckForWinner()
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                if (_balls[i].transform.position.y < -4.7f)
                {
                    _matchRecording.Winner = _matchRecording.BallRecordings[i].Name;
                    _foundWinner = true;
                    Destroy(_map);

                    Time.timeScale = 1f;

                    MatchmakingController.Instance.GetSimulation(_matchRecording);
                    return;
                }
            }
        }

        private void RecordBallPositions()
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                _matchRecording.BallRecordings[i].Positions.Add(_balls[i].transform.position);
            }
        }

        private void DestroyAllBalls()
        {
            foreach (var ball in _balls)
            {
                Destroy(ball);
            }
            _balls.Clear();
        }
    }
}