using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Simple
{
    class Program
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static Orbwalking.Orbwalker Orbwalker;
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Drawing.OnDraw += OnDraw;
        }
        
        private static void OnDraw(EventArgs args)
        {
            Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(null) + 65, System.Drawing.Color.Yellow, 2);
        }
    }
}
