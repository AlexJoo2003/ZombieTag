using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Linq;
using UnityEngine;

namespace ZombieTag.Handlers
{
    class Player
    {
        public static void OnHurting(HurtingEventArgs ev)
        {
            if (ZombieTag.Instance.Config.EventIsOn)
            {
                if (ev.Attacker.IsScp && ZombieTag.Instance.Config.OneHitKO && !ev.Target.IsScp)
                {
                    // if a zombie hurts a human
                    var pos = ev.Target.Position;
                    ev.Target.SetRole(RoleType.Scp0492); //human will be turned into a zombie
                    Timing.CallDelayed(0.5f, () => ev.Target.Position = pos + Vector3.up); // put back where they got turned
                    checkWin(); // and check if there are any humans alive
                    ev.IsAllowed = false; // This prevents any damage recieved by the human, since one hit is already enough
                }
            }
        }

        public static void OnInteractingDoor(InteractingDoorEventArgs ev) // In this game there are doors that u need to have a keycard to open
        {
            if (ZombieTag.Instance.Config.EventIsOn && ev.Player.Role == RoleType.Scp0492)
            {
                if (ev.Door == Map.Doors.First(x => x.Type() == DoorType.GateA) || ev.Door == Map.Doors.First(x => x.Type() == DoorType.GateB))
                {
                    ev.IsAllowed = false; //don't open the escape door
                }
                else
                {
                    ev.IsAllowed = true; //this allows to open every door
                }
            }
        }
        public static void OnDying(DyingEventArgs ev)
        {
            if (ZombieTag.Instance.Config.EventIsOn)
            {
                ev.IsAllowed = false;
                if (ev.Target.Role == RoleType.Scp0492)
                {
                    // If its a zombie who died, They will be put into the void (to prevent glitches) and in 10s will be back in the game at their spawn location
                    ev.Target.SetRole(RoleType.Scp0492);
                    //Timing.CallDelayed(ZombieTag.Instance.Config.ZombieCooldownRespawnTime, () => ev.Target.SetRole(RoleType.Scp0492));
                    Timing.CallDelayed(ZombieTag.Instance.Config.ZombieCooldownRespawnTime + 0.5f, () => ev.Target.Position = Map.Rooms.FirstOrDefault(x => x.Type == RoomType.Hcz049).Position + Vector3.up);
                }
                else
                {
                    // If a human dies by any means they will be turned into a zombie
                    ev.Target.Broadcast(5, "You died, somehow. You are going to be turned into a zombie");
                    Timing.CallDelayed(10f, () =>
                    {
                        ev.Target.SetRole(RoleType.Scp0492);
                        Timing.CallDelayed(0.5f, () => ev.Target.Position = Map.Rooms.FirstOrDefault(x => x.Type == RoomType.Hcz049).Position + Vector3.up);
                        Timing.CallDelayed(7f, () => ev.Target.ShowHint(ZombieTag.Instance.Config.ZombieHintMessage, ZombieTag.Instance.Config.MessageTime));
                    });
                    checkWin();
                }
            }
        }
        public static void OnEscaping(EscapingEventArgs ev)
        {
            if (ZombieTag.Instance.Config.EventIsOn)
            {
                // dont allow escaping, which is imposible in on itself, but just incase
                ev.IsAllowed = false;
            }
        }
        public static void OnJoined(JoinedEventArgs ev)
        {
            if (ZombieTag.Instance.Config.EventIsOn) // Late ppl will be turned to zombies
            {
                ev.Player.Broadcast(5, "You joined late, you are going to be turned into a zombie");
                Timing.CallDelayed(10f, () =>
                {
                    ev.Player.SetRole(RoleType.Scp0492);
                    Timing.CallDelayed(7f, () => ev.Player.ShowHint(ZombieTag.Instance.Config.ZombieHintMessage, ZombieTag.Instance.Config.MessageTime));
                    Timing.CallDelayed(0.5f, () => ev.Player.Position = Map.Rooms.FirstOrDefault(x => x.Type == RoomType.Hcz049).Position + Vector3.up);
                });
            }
        }
        public static void checkWin()
        {
            bool gameOver = true;
            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List)
            {
                if (player.IsHuman)
                {
                    gameOver = false; // game is over only once every human dies
                }
            }
            if (gameOver)
            {
                Map.Broadcast(ZombieTag.Instance.Config.MessageTime, ZombieTag.Instance.Config.ZombiesWinMessage);
                Timing.CallDelayed(ZombieTag.Instance.Config.MessageTime, () => Round.Restart(false)); // end the round declaring the zombie win
                ZombieTag.Instance.Config.EventIsOn = false;
            }
        }

    }
}
