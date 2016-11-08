/*
 * 
 * GG WP
 * 
 */
using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SebbyLib;
using HitChance = SebbyLib.Prediction.HitChance;
using Orbwalking = SebbyLib.Orbwalking;
using Prediction = SebbyLib.Prediction.Prediction;
using PredictionInput = SebbyLib.Prediction.PredictionInput;

namespace SSAurelionSol
{
    internal class Program
    {
        void Main(string[] args)
        {
           CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        #region Declaration

        private static Spell Q, W, E, R;
        private static SebbyLib.Orbwalking.Orbwalker Orbwalker;
        private static Menu Config;

        public static string[] HitchanceNameArray = { "Low", "Medium", "High", "Very High", "Only Immobile" };
        public static HitChance[] HitchanceArray = { HitChance.Low, HitChance.Medium, HitChance.High, HitChance.VeryHigh, HitChance.Immobile };

        private static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        private static int lvl1, lvl2, lvl3, lvl4;
        private static int OuterRange = 650;

        #endregion

        private void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "AurelionSol")
                return;

            #region Spells

            Q = new Spell(SpellSlot.Q, 650f);
            W = new Spell(SpellSlot.W, 650f);
            E = new Spell(SpellSlot.E, 3000f);
            R = new Spell(SpellSlot.R, 1300f);

            Q.SetSkillshot(0.5f, 110f, 850f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.3f, 120f, 4500f, false, SkillshotType.SkillshotLine);

            #endregion

            Config = new Menu("[SS] Aurelion Sol", "SSAurelionSol", true);
            {
                Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalker Settings"));

                var combo = new Menu(":: Combo Settings", ":: Combo Settings");
                {
                    combo.AddItem(new MenuItem("combo.q", "Use Q").SetValue(true));
                    combo.AddItem(new MenuItem("combo.w", "Use W when out of Range").SetValue(true));
                    combo.AddItem(new MenuItem("combo.r", "Use R").SetValue(true));
                    // combo.AddItem(new MenuItem("combo.r.min", "Min. Enemies to use R").SetValue(new Slider(2, 1, 5)));

                    Config.AddSubMenu(combo);
                }

                var lane = new Menu(":: Lane Settings", ":: Lane Settings");
                {
                    lane.AddItem(new MenuItem("lane.q", "Use Q").SetValue(true));
                    lane.AddItem(new MenuItem("lane.min.minions", "Min. Minions to Q").SetValue(new Slider(3, 1, 8)));
                    lane.AddItem(new MenuItem("lane.mana", "Min. Mana Percent").SetValue(new Slider(30, 1, 99)));

                    Config.AddSubMenu(lane);
                }

                var jungle = new Menu(":: Jungle Settings", ":: Jungle Settings");
                {
                    jungle.AddItem(new MenuItem("jungle.q", "Use Q").SetValue(true));
                    jungle.AddItem(new MenuItem("jungle.mana", "Min. Mana Percent").SetValue(new Slider(30, 1, 99)));

                    Config.AddSubMenu(jungle);
                }

                /*
                var lasthit = new Menu(":: Lasthit Settings", ":: Lasthit Settings");
                {
                    lasthit.AddItem(new MenuItem("lasthit.q", "Use Q").SetValue(true));
                    lasthit.AddItem(new MenuItem("lasthit.mana", "Min. Mana Percent").SetValue(new Slider(30, 1, 99)));
                    Config.AddSubMenu(lasthit);
                }
                */

                var killsteal = new Menu(":: KS Settings", ":: KS Settings");
                {
                    killsteal.AddItem(new MenuItem("ks.q", "Use Q").SetValue(true));
                    killsteal.AddItem(new MenuItem("ks.r", "Use R").SetValue(true));

                    Config.AddSubMenu(killsteal);
                }

                var misc = new Menu(":: Misc Settings", ":: Misc Settings");
                {
                    misc.AddItem(new MenuItem("inter.q", "Interrupt (Q)").SetValue(true));
                    misc.AddItem(new MenuItem("gap.q", "Gapclose (Q)").SetValue(true));

                    Config.AddSubMenu(misc);
                }

                var draw = new Menu(":: Draw Settings", ":: Draw Settings");
                {
                    draw.AddItem(new MenuItem("draw.q", "Q Range").SetValue(new Circle(true, Color.Chartreuse)));
                    draw.AddItem(new MenuItem("draw.w", "W Range").SetValue(new Circle(true, Color.Yellow)));
                    draw.AddItem(new MenuItem("draw.e", "E Range").SetValue(new Circle(true, Color.White)));
                    draw.AddItem(new MenuItem("draw.r", "R Range").SetValue(new Circle(true, Color.SandyBrown)));

                    Config.AddSubMenu(draw);
                }

                Config.AddItem(new MenuItem("sol.hitchance", "Skillshot Hitchance").SetValue(new StringList(HitchanceNameArray, 2)));

                var debug = new Menu(":: Debugging", ":: Debugging");
                {
                    debug.AddItem(new MenuItem("debug", "Debug Mode").SetValue(false));
                    debug.AddItem(new MenuItem("debugmouse", "Debug Mode [Mouse]").SetValue(false));
                    debug.AddItem(new MenuItem("warning", "WARNING: May cause FPS issues"));

                    Config.AddSubMenu(debug);
                }
            }

            Config.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
        }

        public void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs arg)
        {
            if (Config.Item("debug").GetValue<bool>())
            {
                if (sender.IsMe)
                {
                    Game.PrintChat("Spell: " + arg.SData.Name);
                    Game.PrintChat("Delay: " + arg.SData.DelayTotalTimePercent.ToString());
                    Game.PrintChat("Cast Range: " + arg.SData.CastRange.ToString());
                    Game.PrintChat("Line Width: " + arg.SData.LineWidth.ToString());
                    Game.PrintChat("Missle Speed: " + arg.SData..ToString());
                    Game.PrintChat("-");
                }
            }
        }

        private void Game_OnUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    ExecuteCombo();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    JungleClear();
                    LaneClear();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
            }
        }

        private void Harass()
        {
            throw new NotImplementedException();
        }

        private void LaneClear()
        {
            throw new NotImplementedException();
        }

        private void JungleClear()
        {
            throw new NotImplementedException();
        }

        private void ExecuteCombo()
        {
            throw new NotImplementedException();
        }
    }
}
