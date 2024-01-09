using NetworkMessages.FromServer;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;

namespace ChatCommands.Commands
{
    class RemoveHorses : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!removeHorses";
        }

        public string Description()
        {
            return "Removes all unmounted horses.";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {

            if(Mission.Current.MountsWithoutRiders.Count == 0)
            {
                GameNetwork.BeginBroadcastModuleEvent();
                GameNetwork.WriteMessage(new ServerMessage("No horses to remove!"));
                GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
            } else {
                foreach (KeyValuePair<Agent, MissionTime> pair in Mission.Current.MountsWithoutRiders)
                {
                    pair.Key?.FadeOut(true, true);
                    
                }
                GameNetwork.BeginBroadcastModuleEvent();
                GameNetwork.WriteMessage(new ServerMessage("Removed all unmounted horses!"));
                GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
            }

            return true;
        }
    }
}
