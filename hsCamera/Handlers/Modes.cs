using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace hsCamera.Handlers
{
    class Modes : Program
    {
        public static void EnemyTracker()
        {
            foreach (var enemy in HeroManager.Enemies.OrderBy(x => x.Distance(ObjectManager.Player.Position))
                .Where(x => x.IsValidTarget(ObjectManager.Player.AttackRange + 100)))
            {
                CameraMovement.SemiDynamic(enemy.Position);
            }
            CameraMovement.SemiDynamic(ObjectManager.Player.Position.Extend(Game.CursorPos, ObjectManager.Player.AttackRange));
        }

        public static void FarmTracker()
        {
            var minions = MinionManager.GetMinions(ObjectManager.Player.Position,
                (ObjectManager.Player.AttackRange + 100),
                MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.Health)
                .OrderBy(x => x.Distance(ObjectManager.Player.Position));

            foreach (var minion in minions)
            {
                CameraMovement.SemiDynamic(minion.Position);
            }
            CameraMovement.SemiDynamic(ObjectManager.Player.Position.Extend(Game.CursorPos, ObjectManager.Player.AttackRange));
        }
    }
}
