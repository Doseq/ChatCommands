using ChatCommands.Commands;
using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ChatCommands
{
    class CommandManager
    {
        public static CommandManager Instance {get;set;}

        public Dictionary<string, Command> Commands;


        public CommandManager() {
            if (CommandManager.Instance == null) {
                
                this.Initialize();
                CommandManager.Instance = this;
            }
        }

        public bool Execute(NetworkCommunicator networkPeer, string command, string[] args)
        {
            var exists = Commands.TryGetValue(command, out var executableCommand);
            if (!exists) {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("This command does not exists", false));
                GameNetwork.EndModuleEventAsServer();
                return false;
            }
            if (!executableCommand.CanUse(networkPeer)) {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("You are not authorized to run this command", false));
                GameNetwork.EndModuleEventAsServer();
                return false;
            }
            return executableCommand.Execute(networkPeer, args);
        }

        private void Initialize() {
            this.Commands = new Dictionary<string, Command>();
            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(Command))))
            {
                var command = (Command) Activator.CreateInstance(mytype);
                if (!Commands.ContainsKey(command.Command())) {
                    Debug.Print("** Chat Command " + command.Command() + " have been initiated !", 0, Debug.DebugColor.Green);
                    Commands.Add(command.Command(), command);
                }
            }

        }
    }
}
