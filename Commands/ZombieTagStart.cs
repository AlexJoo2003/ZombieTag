using System;
using CommandSystem;
using RemoteAdmin;
using MEC;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using UnityEngine;
using Random = System.Random;
using Exiled.API.Features;
using Exiled.API.Extensions;

namespace ZombieTag.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    class ZombieTagStart : ICommand // This is the command that will initiate the event
    {
        public string Command { get; } = "ZombieTagStart";

        public string[] Aliases { get; } = new string[] { "ZTS" };

        public string Description { get; } = "Starts the Zombie Tag event";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var Config = ZombieTag.Instance.Config; // Thats why we needed to create an instance in ZombieTag.cs
            
            if (!Config.EventIsOn) // This is checking if the event is on or of, this command will turn it on, but will do nothing if it is allready turned on.
            {
                Config.EventIsOn = true;

                Map.Doors.First(x => x.Type() == DoorType.GateA).NetworkActiveLocks = 1;
                Map.Doors.First(x => x.Type() == DoorType.GateB).NetworkActiveLocks = 1; // Lock the only 2 escape doors so that survivors has to survive
                                                                                         // Othervise they could just escape which would be too easy

                List<Player> players = new List<Player>();
                var len = 0;
                foreach(Player player in Player.List)
                {
                    players.Add(player);
                    len++;
                }
                Random RNG = new Random();
                Player zombie = players[RNG.Next(0, len)]; // Decide who is going to be the first zombie
                zombie.SetRole(RoleType.Scp0492);
                Timing.CallDelayed(7f, () => zombie.ShowHint(Config.ZombieHintMessage, Config.MessageTime)); // Show him a message on what to do
                Timing.CallDelayed(0.5f, () => zombie.Position = Map.Rooms.FirstOrDefault(x => x.Type == RoomType.Hcz049).Position + Vector3.up); // Spawn him in a specific room
                // I allways seem to have to change their position 0,5s after I changed their role, other wise the game would put them back on the intended place. (which is the void for zombies)

                foreach(Player player in Player.List)
                {
                    if (player.Role != RoleType.Scp0492) // Now for every player which is not a zombie
                    {
                        player.SetRole(RoleType.ClassD); // Turn them into a human
                        player.AddItem(ItemType.MicroHID); // Give them a weapon
                        player.AddItem(ItemType.KeycardO5);
                        Timing.CallDelayed(7f, () => player.ShowHint(Config.HumanHintMessage, Config.MessageTime)); // and show them what to do
                    }
                }

                Map.Broadcast(Config.MessageTime, Config.EventStartedMessage); // Tell the whole server that Zombie Tag event has started

                Timing.CallDelayed(Config.HumanWinTimer, () => // This is the timer that will tell when the survivors win
                {
                    Map.Broadcast(Config.MessageTime, Config.HumanWinMessage); // Tell that D boys survived
                    Config.EventIsOn = false; // Turn off the event
                    Timing.CallDelayed(Config.MessageTime, () => Round.Restart(false)); // restart the round
                }
                );

                response = "Event is ON!";
                return true;
            }
            else
            {
                response = "Event is already on";
                return false;
            }

        }
    }
}
