/*
 * SimulationPlayer.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 31/07/2024
 * Contact: c.dansembourg@icloud.com
 */

using Brop.Views;
using System.Collections.Generic;
using UnityEngine;

namespace Brop
{
    public class SimulationPlayer : MonoBehaviour
    {
        private int _playbackFrameIndex = 0;
        private MatchRecording _matchRecording;
        private List<GameObject> _balls = new List<GameObject>();
        private bool _isPlayingBack = false;
        private GameObject _map;

        public void StartPlayback(MatchRecording matchRecording)
        {
            _map = Instantiate(Resources.Load<GameObject>("Map1"));
            _playbackFrameIndex = 0;
            _isPlayingBack = true;

            _matchRecording = matchRecording;
            foreach (var ballRecording in _matchRecording.BallRecordings)
            {
                var spawnedBall = Instantiate(Resources.Load<GameObject>("PlaybackBall"), ballRecording.Positions[0], Quaternion.identity);
                _balls.Add(spawnedBall);
                spawnedBall.GetComponent<BallView>().SetName(ballRecording.Name);
            }
        }

        private void FixedUpdate()
        {
            if (_isPlayingBack)
                PlayBackRecordedPositions();
        }

        private void PlayBackRecordedPositions()
        {
            if (_playbackFrameIndex >= _matchRecording.BallRecordings[0].Positions.Count)
            {
                _isPlayingBack = false;
                Destroy(_map);
                foreach (var ball in _balls)
                    Destroy(ball);

                MatchmakingController.Instance.MatchFinished(_matchRecording.Winner);
                return;
            }

            for (int i = 0; i < _balls.Count; i++)
                _balls[i].transform.position = _matchRecording.BallRecordings[i].Positions[_playbackFrameIndex];

            _playbackFrameIndex++;
        }
    }
}