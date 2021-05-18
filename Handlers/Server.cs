using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace ZombieTag.Handlers
{
    class Server
    {
        public static void OnWaitingForPlayers()
        {
            Log.Info("Zombie Tag Event is working properly!");
        }
        public static void OnEndingRound(EndingRoundEventArgs ev)
        {
            if (ZombieTag.Instance.Config.EventIsOn) // if the event is on, and the game for some reason would like to end the round, prevent it from happening
            {
                ev.IsAllowed = false;
            }
        }
    }
}
