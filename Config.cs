using System.Collections.Generic;
using System.ComponentModel;

using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace ZombieTag
{
    public class Config : IConfig // This is the config file which the owner can change to their own settings.
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Is the event on for the round or not? keep it on false unless u want to have zombie tag every round")]
        public bool EventIsOn { get; set; } = false; // Since I don't want the event running on at all the time I made another Enabeled Variable to turn it off or on.
                                                     // If I use IsEnabled, if IsEnabled == true the plugin would simply stop wokring.

        [Description("How much time to wait untill declaring human win, set in seconds, default is 600s/10min")]
        public float HumanWinTimer { get; set; } = 10f;

        [Description("Zombies kill humans with one hit")]
        public bool OneHitKO { get; set; } = true;

        [Description("Message that shows up at the start of the event")]
        public string EventStartedMessage { get; set; } = "Zombie Tag Event Has Started!";

        [Description("Message that shows up when humans win")]
        public string HumanWinMessage { get; set; } = "D-boys Win!";

        [Description("Message that shows up when zombies win")]
        public string ZombiesWinMessage { get; set; } = "Zombies Win!";

        [Description("The time in seconds the messages show up on the screen")]
        public ushort MessageTime { get; set; } = 7;

        [Description("Hint that shows to humans")]
        public string HumanHintMessage { get; set; } = "You are a Human. Your task is to survive.";

        [Description("Hint that shows to zombies")]
        public string ZombieHintMessage { get; set; } = "You are a Zombie. Kill humans to get more zombies on your side. KILL EVERYONE!";

        [Description("Set a cooldown for a zombie to respawn if they, die in seconds.")]
        public float ZombieCooldownRespawnTime { get; set; } = 10f;

    }
}