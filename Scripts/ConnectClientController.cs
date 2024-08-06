using Brop.Models;
using Mirror;

namespace Brop
{
    public class ConnectClientController : NetworkBehaviour
    {
        private Player _player;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            _player = new Player()
            {
                Name = "TEST",
                NetworkIdentity = GetComponent<NetworkIdentity>()
            };

            CmdSendPlayer(_player);
            LocalClientController localClientController = FindObjectOfType<LocalClientController>();
            localClientController.ConnectClientController = this;
        }

        public void JoinQueue()
        {
            CmdJoinQueue(_player.NetworkIdentity);
        }

        public void LeaveQueue()
        {
            CmdLeaveQueue(_player.NetworkIdentity);
        }

        [Command]
        public void CmdSendPlayer(Player player)
        {
            PlayersManager.Instance.CreatePlayer(player);
        }

        [Command]
        public void CmdJoinQueue(NetworkIdentity networkIdentity)
        {
            MatchmakingController.Instance.AddPlayerToQueue(networkIdentity);
        }

        [Command]
        public void CmdLeaveQueue(NetworkIdentity networkIdentity)
        {
            MatchmakingController.Instance.RemovePlayerFromQueue(networkIdentity.connectionToClient);
        }
    }
}