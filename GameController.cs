/*
 * GameController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 06/08/2024
 * Contact: c.dansembourg@icloud.com
 */

using UnityEngine;

namespace Brop
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private bool _isHost = false;

        [SerializeField]
        private LocalClientController _clientController;

        public void Start()
        {
            if (_isHost)
                _clientController.ConnectHost();
            else
                _clientController.ConnectClient();
        }
    }
}