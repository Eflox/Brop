/*
 * BallView.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 14/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using Mirror;
using TMPro;
using UnityEngine;

namespace Brop.Views
{
    public class BallView : NetworkBehaviour
    {
        [SerializeField]
        private TMP_Text _playerNameText;

        [SyncVar(hook = nameof(OnNameChanged))]
        private string _playerName;

        public void SetBall(string name)
        {
            _playerName = name;
            _playerNameText.text = name;
        }

        private void OnNameChanged(string oldName, string newName)
        {
            _playerNameText.text = newName;
        }
    }
}