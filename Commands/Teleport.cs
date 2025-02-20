﻿using NetworkMessages.FromServer;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ChatCommands.Commands
{
    class Teleport : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!tp";
        }

        public string Description()
        {
            return "Teleport yourself to another. Usage !tp <Target User>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please provide a username."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            NetworkCommunicator targetPeer = null;
            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.UserName.Contains(string.Join(" ", args)))
                {
                    targetPeer = peer;
                    break;
                }
            }
            if (targetPeer == null)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Target player not found"));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }


            if (networkPeer.ControlledAgent != null && targetPeer.ControlledAgent != null) {
                Vec3 targetPos = targetPeer.ControlledAgent.Position;
                targetPos.x += 1;
                networkPeer.ControlledAgent.TeleportToPosition( targetPos );
            }


            return true;
        }
    }
}
