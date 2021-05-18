using System;

using Exiled.API.Enums;
using Exiled.API.Features;

using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace ZombieTag
{
    public class ZombieTag: Plugin<Config>
    {
        // Create an Instance to easyer access the Config file
        public static ZombieTag Instance { get; } = new ZombieTag();
        private ZombieTag() { }

        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        // This is the handlers I will uses to detect if a player or server did something
        private Handlers.Player player;
        private Handlers.Server server;

        public override void OnEnabled()
        {
            base.OnEnabled();
            RegisterEvents();
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            UnregisterEvents();
        }

        private void RegisterEvents() // U have to register every event when the plugin is turned on
        {
            player = new Handlers.Player();
            server = new Handlers.Server();

            Player.Hurting += Handlers.Player.OnHurting;
            Player.Dying += Handlers.Player.OnDying;
            Player.InteractingDoor += Handlers.Player.OnInteractingDoor;
            Player.Escaping += Handlers.Player.OnEscaping;
            Player.Joined += Handlers.Player.OnJoined;

            Server.WaitingForPlayers += Handlers.Server.OnWaitingForPlayers;
            Server.EndingRound += Handlers.Server.OnEndingRound;
        }
        private void UnregisterEvents() // U have to unregister it to prevent lag
        {
            Player.Hurting -= Handlers.Player.OnHurting;
            Player.Dying -= Handlers.Player.OnDying;
            Player.InteractingDoor -= Handlers.Player.OnInteractingDoor;
            Player.Escaping -= Handlers.Player.OnEscaping;
            Player.Joined -= Handlers.Player.OnJoined;

            Server.WaitingForPlayers -= Handlers.Server.OnWaitingForPlayers;
            Server.EndingRound -= Handlers.Server.OnEndingRound;

            player = null;
            server = null;
        }
    }
}
