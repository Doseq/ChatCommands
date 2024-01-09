using NetworkMessages.FromServer;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ChatCommands.Commands
{
    class TeleportAllToMe : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!tpalltome";
        }

        public string Description()
        {
            return "Teleport all players to you. Usage !tpalltome <Target User>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {

                if (peer.ControlledAgent != null && peer.ControlledAgent != null)
                {
                    Vec3 targetPos = networkPeer.ControlledAgent.Position;
                    targetPos.x += 1;
                    peer.ControlledAgent.TeleportToPosition(targetPos);
                }
            }

            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage("All players teleported to " + networkPeer.UserName));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
            return true;
        }
    }
}
