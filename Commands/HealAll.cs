using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade;

namespace ChatCommands.Commands
{
    class HealAll : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!healall";
        }

        public string Description()
        {
            return "Healing all players and horses (if mounted).";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.ControlledAgent != null)
                {
                    peer.ControlledAgent.Health = peer.ControlledAgent.HealthLimit;
                    if (peer.ControlledAgent.HasMount)
                    {
                        peer.ControlledAgent.MountAgent.Health = peer.ControlledAgent.MountAgent.HealthLimit;
                    }
                }
            }

            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage("All players and their horses are healed"));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
            return true;
        }
    }
}
