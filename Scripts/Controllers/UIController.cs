/*
 * UIController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 19/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brop
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private Button _connectButton;

        [SerializeField]
        private TMP_InputField _nameLabel;

        [SerializeField]
        private GameObject _menuUI;

        [SerializeField]
        private GameObject _searchUI;

        public static string PlayerName { get; private set; } // Store player name

        private void Start()
        {
            _connectButton.onClick.AddListener(OnConnectButtonClicked);
        }

        private void OnConnectButtonClicked()
        {
            PlayerName = _nameLabel.text; // Set player name

            _menuUI.SetActive(false);
            _searchUI.SetActive(true);

            Debug.Log("Connect button clicked");

            if (NetworkServer.active)
            {
                Debug.Log("You are the host and cannot connect as a player.");
                return;
            }

            if (NetworkClient.isConnected)
            {
                Debug.Log("Already connected to a server.");
                MenuUIController.Instance.CmdSearchForMatch(PlayerName);
                return;
            }

            Debug.Log("Starting client...");
            NetworkManager.singleton.StartClient();
        }
    }
}