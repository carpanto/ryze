using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            CustomEvents.Game.OnGameLoad += eventArgs => Drawing.OnDraw += args1 => Render.Circle.DrawCircle(Player.Position, Player.AttackRange, Color.AliceBlue);
        }
    }
}
