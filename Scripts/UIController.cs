/*
 * UIController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 19/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brop
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private bool _dontOpenInitialUI = false;

        [SerializeField]
        private LocalClientController _localClientController;

        [Header(header: "UI's")]
        [SerializeField]
        private GameObject _menuUI;

        [SerializeField]
        private GameObject _serverUI;

        [SerializeField]
        private GameObject _debugUI;

        [SerializeField]
        private GameObject _loadingUI;

        [Header("Buttons")]
        [SerializeField]
        private Button _joinQeueButton;

        [SerializeField]
        private Button _connectHostButton;

        [SerializeField]
        private Button _connectClientButton;

        [SerializeField]
        private Button _stopHostingButton;

        [SerializeField]
        private Button _cancelLoadingButton;

        [SerializeField]
        private Button _generateNameButton;

        [SerializeField]
        private Button _disconnectClientButton;

        [Header("Text")]
        [SerializeField]
        private TMP_Text _playersOnlineText;

        [SerializeField]
        private TMP_Text _playersInQeueText;

        [SerializeField]
        private TMP_Text _nameText;

        [SerializeField]
        private TMP_Text _loadingText;

        [SerializeField]
        private TMP_Text _loadingButtonText;

        private Dictionary<UI, GameObject> _UIs;

        public void UpdatePlayerCount(int playerCount)
        {
            _playersOnlineText.text = $"{playerCount} Players Online";
        }

        public void UpdatePlayerInQeueCount(int playerCount)
        {
            _playersInQeueText.text = $"{playerCount}";
        }

        public void ServerConnected()
        {
            Open(UI.Server);
        }

        public void ServerDisconnected()
        {
            Open(UI.Debug);
        }

        public void ClientConnected()
        {
            Open(UI.Menu);
        }

        public void ClientDisconnected()
        {
            Open(UI.Debug);
        }

        public void ShowWinner(string winner)
        {
            OpenLoading($"{winner} WINNER!", UI.Menu, "Return");
        }

        public void SetName(string name)
        {
            _nameText.text = name;
        }

        private void Awake()
        {
            _UIs = new Dictionary<UI, GameObject>
            {
                { UI.Menu, _menuUI },
                { UI.Server, _serverUI },
                { UI.Debug, _debugUI },
                { UI.Loading, _loadingUI },
                //{ UI.Winner, _winnerUI },
            };

            _joinQeueButton.onClick.AddListener(OnJoinQeueButton);
            _connectHostButton.onClick.AddListener(OnConnectHostButton);
            _connectClientButton.onClick.AddListener(OnConnectClientButton);
            _stopHostingButton.onClick.AddListener(OnStopHosting);
            _disconnectClientButton.onClick.AddListener(OnStopClient);

            if (!_dontOpenInitialUI)
                Open(UI.Debug);
        }

        private void Open(UI uiToOpen)
        {
            foreach (var ui in _UIs.Values)
                ui.SetActive(false);

            _UIs[uiToOpen].SetActive(true);
        }

        public void CloseAll()
        {
            foreach (var ui in _UIs.Values)
                ui.SetActive(false);
        }

        public void LoadMatch()
        {
            OpenLoading("Loading match...", UI.Menu, "Cancel");
        }

        private void OpenLoading(string text, UI cancelUI, string loadingButtonText, Action optionalAction = null)
        {
            _loadingText.text = text;
            _loadingButtonText.text = loadingButtonText;

            _cancelLoadingButton.onClick.AddListener(() =>
            {
                Open(cancelUI);
                if (optionalAction != null)
                    optionalAction.Invoke();

                _cancelLoadingButton.onClick.RemoveAllListeners();
            });

            Open(UI.Loading);
        }

        private void OnJoinQeueButton()
        {
            OpenLoading("Searching for match...", UI.Menu, "Cancel", () => _localClientController.LeaveQueue());

            _localClientController.JoinQueue();
        }

        private void OnConnectHostButton()
        {
            OpenLoading("Starting host...", UI.Debug, "Cancel");
            _localClientController.ConnectHost();
        }

        private void OnConnectClientButton()
        {
            OpenLoading("Connecting to client...", UI.Debug, "Cancel");
            _localClientController.ConnectClient();
        }

        private void OnStopHosting()
        {
            _localClientController.DisconnectHost();

            Open(UI.Debug);
        }

        private void OnStopClient()
        {
            _localClientController.DisconnectClient();

            Open(UI.Debug);
        }
    }

    public enum UI
    {
        Menu,
        Server,
        Debug,
        Loading,
        Winner
    }
}