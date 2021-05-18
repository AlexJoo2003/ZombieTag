using System;
using CommandSystem;
using RemoteAdmin;
using MEC;

namespace ZombieTag.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    class ZombieTagEnd : ICommand
    {
        public string Command { get; } = "ZombieTagEnd";

        public string[] Aliases { get; } = new string[] { "ZTE" };

        public string Description { get; } = "Ends the Zombie Tag event";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var Config = ZombieTag.Instance.Config; // you can turn off the event

            if (Config.EventIsOn)
            {
                Config.EventIsOn = false;

                response = "Event is OFF!";
                return true;
            }
            else
            {
                response = "Event is already off.";
                return false;
            }
        }
    }
}
