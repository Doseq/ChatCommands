using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade;

namespace ChatCommands.Commands
{
    class HealAll : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!healall";
        }

        public string Description()
        {
            return "Heals all players and horses (if mounted) + repairs players shields.";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.ControlledAgent != null)
                {
                    peer.ControlledAgent.Health = peer.ControlledAgent.HealthLimit;
                    peer.ControlledAgent.RestoreShieldHitPoints();
                    if (peer.ControlledAgent.HasMount)
                    {
                        peer.ControlledAgent.MountAgent.Health = peer.ControlledAgent.MountAgent.HealthLimit;
                    }
                }
            }

            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage("All players with their horses are healed, all shields are repaired."));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
            return true;
        }
    }
}
