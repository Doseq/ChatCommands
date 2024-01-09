using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ChatCommands
{
    public class ChatCommandsSubModule : MBSubModuleBase
    {
        public static ChatCommandsSubModule Instance { get; private set; }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Debug.Print("** CHAT COMMANDS BY MENTALROB LOADED **", 0, Debug.DebugColor.Green);

            CommandManager cm = new CommandManager();
        }

        protected override void OnSubModuleUnloaded() {
            Debug.Print("** CHAT COMMANDS BY MENTALROB UNLOADED **", 0, Debug.DebugColor.Green);
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject) {
            Debug.Print("** CHAT HANDLER ADDED **", 0, Debug.DebugColor.Green);
            game.AddGameHandler<ChatHandler>();
        }

        public override void OnGameEnd(Game game) {
            game.RemoveGameHandler<ChatHandler>();
        }

    }
}
