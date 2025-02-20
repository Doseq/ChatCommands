﻿using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade;

namespace ChatCommands.Commands
{
    class HealMe : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!healme";
        }

        public string Description()
        {
            return "Healing yourself.";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (networkPeer.ControlledAgent != null)
            {
                networkPeer.ControlledAgent.Health = networkPeer.ControlledAgent.HealthLimit;

            }
            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage("Healing yourself"));
            GameNetwork.EndModuleEventAsServer();
            return true;
        }
    }
}
