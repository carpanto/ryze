using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace TroopGaren
{
    internal class Program
    {
        private static Orbwalking.Orbwalker Orbwalker;

        //Menu
        private static Menu Menu;

        public static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Menu = new Menu("Orbwalker", "Orbwalker", true);
            var orbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            Menu.AddToMainMenu();
        }
    }
}