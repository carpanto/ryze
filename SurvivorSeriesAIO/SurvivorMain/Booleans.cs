namespace SurvivorSeriesAIO.SurvivorMain
{
    class Booleans
    {
        /// <summary>
        /// Booleans for each OrbWalker Mode
        /// </summary>
        public static bool LaneClear { get { return (Program.Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.LaneClear); } }

        public static bool Combo { get { return (Program.Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.Combo); } }

        public static bool Mixed { get { return (Program.Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.Mixed); } }

        public static bool None { get { return (Program.Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.None); } }
    }
}
