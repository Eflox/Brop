/*
 * MenuUIController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 14/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using Mirror;
using UnityEngine;

namespace Brop
{
    public class MenuUIController : NetworkBehaviour
    {
        public static MenuUIController Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [Command]
        public void CmdSearchForMatch(string name)
        {
            if (NetworkClient.connection != null)
            {
                MatchmakingServerController.Instance.SearchForMatch(name, NetworkClient.connection);
            }
            else
            {
                Debug.LogError("Client connection is null");
            }
        }
    }
}