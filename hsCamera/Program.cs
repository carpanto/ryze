using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace hsCamera
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += HsCameraOnLoad;
        }

        private static Menu _config;

        private static void HsCameraOnLoad(EventArgs args)
        {
            _config = new Menu("hsCamera", "hsCamera", true);
            {
                _config.AddItem(new MenuItem("show.enemy.1", "Show Enemy 1 Location").SetValue(new KeyBind(97, KeyBindType.Press)));
                _config.AddItem(new MenuItem("show.enemy.2", "Show Enemy 2 Location").SetValue(new KeyBind(98, KeyBindType.Press)));
                _config.AddItem(new MenuItem("show.enemy.3", "Show Enemy 3 Location").SetValue(new KeyBind(99, KeyBindType.Press)));
                _config.AddItem(new MenuItem("show.enemy.4", "Show Enemy 4 Location").SetValue(new KeyBind(100, KeyBindType.Press)));
                _config.AddItem(new MenuItem("show.enemy.5", "Show Enemy 5 Location").SetValue(new KeyBind(101, KeyBindType.Press)));
                _config.AddItem(new MenuItem("semi.dynamic", "Semi-Dynamic Camera ?").SetValue(false));
                _config.AddToMainMenu();
            }
            Game.OnUpdate += HsCameraOnUpdate;
        }

        private static void HsCameraOnUpdate(EventArgs args)
        {
            if (_config.Item("show.enemy.1").GetValue<KeyBind>().Active)
            {
                var enemylist = HeroManager.Enemies.ToList();
                if (enemylist[0].IsValid && enemylist[0] != null && enemylist[0].IsChampion())
                {
                    var position = enemylist[0].Position;
                    Camera.Position = position;
                }
            }
            if (_config.Item("show.enemy.2").GetValue<KeyBind>().Active)
            {
                var enemylist = HeroManager.Enemies.ToList();
                if (enemylist[1].IsValid && enemylist[1] != null && enemylist[1].IsChampion())
                {
                    var position = enemylist[1].Position;
                    Camera.Position = position;
                }
            }
            if (_config.Item("show.enemy.3").GetValue<KeyBind>().Active)
            {
                var enemylist = HeroManager.Enemies.ToList();
                if (enemylist[2].IsValid && enemylist[2] != null && enemylist[2].IsChampion())
                {
                    var position = enemylist[2].Position;
                    Camera.Position = position;
                }
            }
            if (_config.Item("show.enemy.4").GetValue<KeyBind>().Active)
            {
                var enemylist = HeroManager.Enemies.ToList();
                if (enemylist[3].IsValid && enemylist[3] != null && enemylist[3].IsChampion())
                {
                    var position = enemylist[3].Position;
                    Camera.Position = position;
                }
            }
            if (_config.Item("show.enemy.5").GetValue<KeyBind>().Active)
            {
                var enemylist = HeroManager.Enemies.ToList();
                if (enemylist[4].IsValid && enemylist[4] != null && enemylist[4].IsChampion())
                {
                    var position = enemylist[4].Position;
                    Camera.Position = position;
                }
            }

            if (_config.Item("semi.dynamic").GetValue<bool>())
            {
                SemiDynamic(ObjectManager.Player.Position);
            }
        }

        private static void SemiDynamic(Vector3 position)
        {
            var distance = Camera.Position.Distance(position);


            if (distance <= 1)
            {
                return;
            }

            var speed = Math.Max(0.2f, Math.Min(20, distance * 0.0007f * 20));
            var direction = (position - Camera.Position).Normalized() * (speed);

            Camera.Position = direction + Camera.Position;
        }
    }
}
