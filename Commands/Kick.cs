﻿using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;

namespace ChatCommands.Commands
{
    class Kick : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!kick";
        }

        public string Description()
        {
            return "Kicks a player. Caution ! First user that contains the provided input will be kicked. Usage !kick <Player Name>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please provide a username. Player that contains provided input will be kicked."));
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

            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage("Player " + targetPeer.UserName + " is kicked from the server"));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);

            DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(targetPeer.VirtualPlayer.Id, false);
            return true;
        }
    }
}

