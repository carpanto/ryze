namespace Simple
{
    using LeagueSharp;
    using LeagueSharp.Common;

    class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs => Drawing.OnDraw += args1 => Render.Circle.DrawCircle(ObjectManager.Player.Position, ObjectManager.Player.AttackRange, System.Drawing.Color.AliceBlue);
        }
    }
}
