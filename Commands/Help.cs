﻿using NetworkMessages.FromServer;
using System.Linq;
using TaleWorlds.MountAndBlade;

namespace ChatCommands.Commands
{
    class Help : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return true;
        }

        public string Command()
        {
            return "!help";
        }

        public string Description()
        {
            return "Help message";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            string[] commands = CommandManager.Instance.Commands.Keys.ToArray();
            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage("-==== Command List ===-"));
            GameNetwork.EndModuleEventAsServer();

            foreach (string command in commands) {
                Command commandExecutable = CommandManager.Instance.Commands[command];
                if(commandExecutable.CanUse(networkPeer))
                {
                    GameNetwork.BeginModuleEventAsServer(networkPeer);
                    GameNetwork.WriteMessage(new ServerMessage(command + ": " + commandExecutable.Description()));
                    GameNetwork.EndModuleEventAsServer();
                }
            }
            return true;
        }
    }
}
