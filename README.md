# Chat Commands

Chat commands is a server side mod that allow players to use chat commands

## Installation

- Download the latest release (It's located on right panel)
- Add the following xml node to your `[Dedicated Server Files]/Modules/Multiplayer/SubModule.xml` file, between the  `<SubModules> </SubModules>` tags. 
```xml
<SubModule>
    <Name value="ChatCommands" />
    <DLLName value="ChatCommands.dll" />
    <SubModuleClassType value="ChatCommands.ChatCommandsSubModule" />
    <Tags>
        <Tag key="DedicatedServerType" value="custom" />
    </Tags>
</SubModule>
```
- Your SubModule.xml file should look like this
```xml
<?xml version="1.0" encoding="utf-8"?>
<Module>
	<Name value="Multiplayer" />
	<Id value="Multiplayer" />
	<Version value="e1.8.0" />
	<DefaultModule value="true" />
	<ModuleCategory value="Multiplayer" />
	<Official value="true" />
	<DependedModules>
		<DependedModule Id="Native" DependentVersion="e1.8.0" Optional="false" />
	</DependedModules>
	<SubModules>
        <!-- Added here -->
        <SubModule>
            <Name value="ChatCommands" />
            <DLLName value="ChatCommands.dll" />
            <SubModuleClassType value="ChatCommands.ChatCommandsSubModule" />
            <Tags>
                <Tag key="DedicatedServerType" value="custom" />
            </Tags>
        </SubModule>
        <!-- Your other submodules -->
	</SubModules>
</Module>
```

## Usage

While in the server you can open up the chat and type `!help` command to see what commands you can use.
Requirement is to log in into server using admin password (functionality provided in BL 1.2 along with AdminPanel).

## I want to create my own command.

Create a class that implements `Command` interface. And voila you have your own command. At the server startup it's automagically detects the `Command` implemented classes and appends it to registry. Lets examine an example.

```csharp
namespace ChatCommands.Commands
{
    class GodMode : Command // This is our class that implements `Command` interface.
    {
        // Here you can define who can use this command.
        public bool CanUse(NetworkCommunicator networkPeer) 
        {
            // Check if the user is admin
            return networkPeer.isAdmin;
        }
        
        // Define the command here. Note that mod only filters command that starts with `!`
        public string Command()
        {
            return "!godmode"; 
        }

        // Your command's description.
        public string Description()
        {
            return "Ascend yourself. Be something selestial"; 
        }

        // Execution phase of the command.
        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // networkPeer: issuer of the command
            // args: arguments of the command.
            // If a user types `!godmode blabla` `args[0]` will be `blabla`
            if (networkPeer.ControlledAgent != null) {
                networkPeer.ControlledAgent.BaseHealthLimit = 2000;
                networkPeer.ControlledAgent.HealthLimit = 2000;
                networkPeer.ControlledAgent.Health = 2000;
                networkPeer.ControlledAgent.SetMinimumSpeed(10);
                networkPeer.ControlledAgent.SetMaximumSpeedLimit(10, false);
                
            }
            return true;
        }
    }
}
```

For more example you can check other commands.

If you want to contribute you can, if you want me to implement a command, please open up an issue.

## License
[MIT](https://choosealicense.com/licenses/mit/)

# Thanks to
- Horns
- Falcomfr
