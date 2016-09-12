/*
 *
 * Thanks to imsosharp for the Q to get closer to enemy if he's not in your Q Range (using minions to gapclose) [Code Snippet - Simple Champion, no need for 1289321 logics] 
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data;
using SharpDX;
using SebbyLib;
using Orbwalking = SebbyLib.Orbwalking;

namespace SVIrelia
{
    class Program
    {
        /// <summary>
        /// Declarations
        /// </summary>
        public static Spell Q, W, E, R;
        public static Menu Menu;
        public static SebbyLib.Orbwalking.Orbwalker Orbwalker;

        /// <summary>
        /// ObjectManager => Player
        /// </summary>
        public static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        /// <summary>
        /// Run Irelia script on gameload
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Irelia;
        }

        /// <summary>
        /// Main Module
        /// </summary>
        /// <param name="args"></param>
        private static void Irelia(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Irelia")
                return;

            Q = new Spell(SpellSlot.Q, 650);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 425);
            R = new Spell(SpellSlot.R, 1000);
            R.SetSkillshot(0.25f, 45, 1600, false, SkillshotType.SkillshotLine);

            Init();
            Game.PrintChat("<font color='#800040'>[SurvivorSeries] Irelia</font> <font color='#ff6600'>Loaded.</font>");
            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnDoCast += WUsage;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        /// <summary>
        /// W Reset/Usage - Improved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void WUsage(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.IsAutoAttack() && args.Target is Obj_AI_Hero &&
                Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && W.IsReady())
            {
                W.Cast();
            }
        }

        /// <summary>
        /// Initialize Extens
        /// </summary>
        public static void Init()
        {
            Menu = new Menu(":: SurvivorIrelia", "SurvivorIrelia", true);
            {
                var orbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "orbwalker"));
                Orbwalker = new SebbyLib.Orbwalking.Orbwalker(orbwalkerMenu);
                var combomenu = new Menu(":: Combo Settings", "combosettings");
                {
                    combomenu.AddItem(new MenuItem("q.combo", "Use (Q)")).SetValue(true);
                    combomenu.AddItem(new MenuItem("w.combo", "Use (W)")).SetValue(true);
                    combomenu.AddItem(new MenuItem("e.combo", "Use (E)")).SetValue(true);
                    combomenu.AddItem(new MenuItem("r.combo", "Use (R)")).SetValue(true);
                    combomenu.AddItem(
                        new MenuItem("rinfo", ":: R Info").SetTooltip(
                                "It'll cast R's at enemy once you've activated the ult once.")
                            .SetFontStyle(FontStyle.Bold, SharpDX.Color.DeepPink));
                    combomenu.AddItem(new MenuItem("combo.style", "(COMBO STYLE) -> ").SetValue(new StringList(new[] { "Normal", "Burst" })).SetTooltip("Soon TM!"))
                     .ValueChanged += (s, ar) =>
                     {
                         combomenu.Item("burst.enemy.killable.check").Show(ar.GetNewValue<StringList>().SelectedIndex == 0);
                     };
                    combomenu.AddItem(new MenuItem("burst.enemy.killable.check", "Only Enemy Killable")
                        .SetValue(true).SetFontStyle(System.Drawing.FontStyle.Bold))
                        .Show(combomenu.Item("combo.style").GetValue<StringList>().SelectedIndex == 0);
                    Menu.AddSubMenu(combomenu);
                }
                var laneclearmenu = new Menu(":: LaneClear Settings", "laneclearsettings");
                {
                    laneclearmenu.AddItem(new MenuItem("LCQ", "Use (Q)")).SetValue(true);
                    laneclearmenu.AddItem(new MenuItem("LCW", "Use (W)")).SetValue(true);
                    laneclearmenu.AddItem(new MenuItem("LCWSlider", "Minimum Minions in Range to (W)"))
                        .SetValue(new Slider(2, 0, 10));
                    laneclearmenu.AddItem(new MenuItem("LCE", "Use (E)")).SetValue(false);
                    Menu.AddSubMenu(laneclearmenu);
                }
                var harassmenu = new Menu(":: Harass Settings", "harasssettings");
                {
                    harassmenu.AddItem(new MenuItem("HarassQ", "Use (Q)")).SetValue(true);
                    harassmenu.AddItem(new MenuItem("HarassE", "Use (E)")).SetValue(true);
                    harassmenu.AddItem(
                        new MenuItem("HarassManaManager", "Harass Mana Manager (%)").SetValue(new Slider(30, 0, 100)));
                    Menu.AddSubMenu(harassmenu);
                }
                var drawingmenu = new Menu(":: Drawings Menu", "drawingsmenu");
                {
                    drawingmenu.AddItem(new MenuItem("DrawAA", "Draw (AA) Range").SetValue(false));
                    drawingmenu.AddItem(new MenuItem("DrawQ", "Draw (Q) Range").SetValue(true));
                    drawingmenu.AddItem(new MenuItem("DrawR", "Draw (R) Range").SetValue(true));
                    Menu.AddSubMenu(drawingmenu);
                }
                var miscmenu = new Menu(":: Misc Menu", "miscmenu");
                {
                    miscmenu.AddItem(new MenuItem("HitChance", "Hit Chance").SetValue(new StringList(new[] { "Medium", "High", "Very High" }, 1)));
                    Menu.AddSubMenu(miscmenu);
                }

                #region DrawHPDamage
                var drawdamage = new Menu(":: Draw Damage", "drawdamage");
                {
                    var dmgAfterShave =
                        new MenuItem("SurvivorAshe.DrawComboDamage", "Draw Damage on Enemy's HP Bar").SetValue(true);
                    var drawFill =
                        new MenuItem("SurvivorAshe.DrawColour", "Fill Color", true).SetValue(
                            new Circle(true, System.Drawing.Color.Chartreuse));
                    drawdamage.AddItem(drawFill);
                    drawdamage.AddItem(dmgAfterShave);
                    DrawDamage.DamageToUnit = CalculateDamage;
                    DrawDamage.Enabled = dmgAfterShave.GetValue<bool>();
                    DrawDamage.Fill = drawFill.GetValue<Circle>().Active;
                    DrawDamage.FillColor = drawFill.GetValue<Circle>().Color;
                    dmgAfterShave.ValueChanged +=
                        delegate(object sender, OnValueChangeEventArgs eventArgs)
                        {
                            DrawDamage.Enabled = eventArgs.GetNewValue<bool>();
                        };

                    drawFill.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
                    {
                        DrawDamage.Fill = eventArgs.GetNewValue<Circle>().Active;
                        DrawDamage.FillColor = eventArgs.GetNewValue<Circle>().Color;
                    };
                }
                #endregion
                Menu.AddToMainMenu();
            }
        }

        /// <summary>
        /// CalculateDamage Amount
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns></returns>
        private static float CalculateDamage(Obj_AI_Hero enemy)
        {
            float damage = 0;

            if (Q.IsReady() && Player.Mana > Q.Instance.ManaCost)
            {
                damage += Q.GetDamage(enemy);
            }
            if (W.IsReady() && Player.Mana > W.Instance.ManaCost)
            {
                damage += W.GetDamage(enemy);
            }
            if (E.IsReady() && Player.Mana > E.Instance.ManaCost)
            {
                damage += E.GetDamage(enemy);
            }
            if (HasRBuff() || R.IsReady())
            {
                damage += R.GetDamage(enemy);
            }
            damage += (float)Player.GetAutoAttackDamage(enemy);

            return damage;
        }

        /// <summary>
        /// OnUpdate Event
        /// </summary>
        /// <param name="args"></param>
        private static void OnUpdate(EventArgs args)
        {
            var target = TargetSelector.GetTarget(900, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget())
                return;

            switch (Orbwalker.ActiveMode)
            {
                    case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
                    case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    break;
                    case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
            }
        }

        /// <summary>
        /// Drawings
        /// </summary>
        /// <param name="args"></param>
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Menu.Item("DrawAA").GetValue<bool>())
                Render.Circle.DrawCircle(ObjectManager.Player.Position, SebbyLib.Orbwalking.GetRealAutoAttackRange(null), System.Drawing.Color.BlueViolet);
            if (Menu.Item("DrawQ").GetValue<bool>())
                Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.Chartreuse);
            if (Menu.Item("DrawR").GetValue<bool>() && R.Level > 0 && R.Instance.IsReady())
                Render.Circle.DrawCircle(ObjectManager.Player.Position, R.Range, System.Drawing.Color.DeepPink);
        }

        /// <summary>
        /// Player has RBuff :?
        /// </summary>
        /// <returns></returns>
        public static bool HasRBuff() => Player.HasBuff("ireliatranscendentbladesspell");

        /// <summary>
        /// Combo Mode
        /// </summary>
        public static void Combo()
        {
            var useq = Menu.Item("q.combo").GetValue<bool>();
            var usew = Menu.Item("w.combo").GetValue<bool>();
            var usee = Menu.Item("e.combo").GetValue<bool>();
            var useR = Menu.Item("r.combo").GetValue<bool>();
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            //var targetextend = TargetSelector.GetTarget();
            if (target == null || !target.IsValidTarget())
                return;

            var distancebetweenmeandtarget = ObjectManager.Player.ServerPosition.Distance(target.ServerPosition);
            if (Q.IsReady() && target.Distance(Player.Position) > 650)
            {
                var gapclosingMinion = ObjectManager.Get<Obj_AI_Minion>().Where(m => m.ServerPosition.Distance(ObjectManager.Player.ServerPosition) < 650 &&
                    m.IsEnemy && m.ServerPosition.Distance(target.ServerPosition) < distancebetweenmeandtarget && m.Health > 1 && m.Health < Q.GetDamage(m)).OrderBy(m => m.Position.Distance(target.ServerPosition)).FirstOrDefault();
                if (gapclosingMinion != null)
                {
                    Q.Cast(gapclosingMinion);
                }
            }

            if (target.IsValidTarget(Q.Range) && !target.IsValidTarget(Player.AttackRange) && Q.IsReady() && useq)
            {
                Q.CastOnUnit(target);
                if (E.IsReady() && target.IsValidTarget(E.Range) && usee)
                    E.CastOnUnit(target);
                if (W.IsReady() && usew && target.IsValidTarget(Player.AttackRange)) // Yes lads E.Range is intentionally here :gosh:
                    W.Cast();
                if (target.IsValidTarget(R.Range) && useR)
                {
                    var pred = R.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High && HasRBuff())
                    {
                        if (OktwCommon.CollisionYasuo(Player.Position, pred.CastPosition))
                            return;
                        R.Cast(pred.CastPosition);
                    }
                }
            }
            else if (target.IsValidTarget(E.Range))
            {
                if (Q.IsReady() && useq)
                    Q.CastOnUnit(target);
                if (E.IsReady() && usee && target.IsValidTarget(E.Range))
                    E.CastOnUnit(target);
                if (W.IsReady() && usew && target.IsValidTarget(Player.AttackRange)) // Yes lads E.Range is intentionally here :gosh:
                    W.Cast();
                if (R.IsReady() && useR && target.IsValidTarget(R.Range))
                {
                    var pred = R.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        if (OktwCommon.CollisionYasuo(Player.Position, pred.CastPosition))
                            return;
                        R.Cast(pred.CastPosition);
                    }
                }
            }
        }

        /// <summary>
        /// LaneClear Mode
        /// </summary>
        public static void LaneClear()
        {
            var lcq = Menu.Item("LCQ").GetValue<bool>();
            var lcw = Menu.Item("LCW").GetValue<bool>();
            var lce = Menu.Item("LCE").GetValue<bool>();
            var lcwslider = Menu.Item("LCWSlider").GetValue<Slider>().Value;

            var allMinions =
                MinionManager.GetMinions(Q.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.Health);

            foreach (var minion in allMinions)
            {
                if (minion.IsValidTarget() && minion.Health < Q.GetDamage(minion) && Q.IsReady() && lcq)
                    Q.CastOnUnit(minion);

                if (allMinions.Count > lcwslider && W.IsReady() && lcw && minion.HealthPercent > 5 && minion.IsValidTarget())
                    W.Cast();
            }
        }

        /// <summary>
        /// Harass Mode
        /// </summary>
        public static void Harass()
        {
            if (Player.ManaPercent < Menu.Item("HarassManaManager").GetValue<Slider>().Value)
                return;

            var harassq = Menu.Item("HarassQ").GetValue<bool>();
            var harasse = Menu.Item("HarassE").GetValue<bool>();

            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValidTarget())
                return;

            if (Q.IsReady() && harassq)
            {
                Q.CastOnUnit(target);
                if (harasse && E.IsReady())
                {
                    E.CastOnUnit(target);
                    Orbwalker.ForceTarget(target);
                }
                else
                {
                    Orbwalker.ForceTarget(target);
                }
            }

            if (harasse && E.IsReady() && !Q.IsReady())
            {
                if (target.IsValidTarget(E.Range))
                {
                    E.CastOnUnit(target);
                    Orbwalker.ForceTarget(target);
                }
            }
        }
    }
}
