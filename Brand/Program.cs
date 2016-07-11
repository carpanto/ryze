/**
 * 
 * Credits: Sebby for the Lib and Brand R Logic + few other stuff (Since I'm "newbie" to his Lib :kappa:)
 * 
 **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data;
using SebbyLib;
using SharpDX;
using Color = System.Drawing.Color;

namespace SurvivorBrand
{
    class Program
    {

        #region Declaration
        private static Spell Q, W, E, R;
        private static float RManaC = 0;
        private static SpellSlot IgniteSlot;
        private static SebbyLib.Orbwalking.Orbwalker Orbwalker;
        private static Menu Menu;
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        public const string ChampionName = "Brand";
        #endregion
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != ChampionName)
                return;

            #region Spells
            Q = new Spell(SpellSlot.Q, 1050f);
            W = new Spell(SpellSlot.W, 900f);
            E = new Spell(SpellSlot.E, 625f);
            R = new Spell(SpellSlot.R, 750f);
            Q.SetSkillshot(0.25f, 60f, 1600f, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(1.15f, 230f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            R.SetTargetted(0.25f, 2000f);
            #endregion

            #region SummonerSpells
            IgniteSlot = Player.GetSpellSlot("summonerdot");
            #endregion

            #region Menu
            Menu = new Menu("SurvivorBrand", "SurvivorBrand", true);

            Menu OrbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new SebbyLib.Orbwalking.Orbwalker(OrbwalkerMenu);

            Menu TargetSelectorMenu = Menu.AddSubMenu(new Menu("Target Selector", "TargetSelector"));
            TargetSelector.AddToMenu(TargetSelectorMenu);

            Menu ComboMenu = Menu.AddSubMenu(new Menu("Combo", "Combo"));
            ComboMenu.AddItem(new MenuItem("ComboUseQ", "Use Q").SetValue(true));
            ComboMenu.AddItem(new MenuItem("ComboUseW", "Use W").SetValue(true));
            ComboMenu.AddItem(new MenuItem("ComboUseE", "Use E").SetValue(true));
            ComboMenu.AddItem(new MenuItem("ComboUseR", "Use R").SetValue(true));
            // ComboMenu.AddItem(new MenuItem("ComboUseIgnite", "Use Ignite").SetValue(true));

            Menu HarassMenu = Menu.AddSubMenu(new Menu("Harass", "Harass"));
            HarassMenu.AddItem(new MenuItem("harassQ", "Use Q").SetValue(true));
            HarassMenu.AddItem(new MenuItem("harassW", "Use W").SetValue(true));
            HarassMenu.AddItem(new MenuItem("harassE", "Use E").SetValue(true));
            HarassMenu.AddItem(new MenuItem("HarassManaManager", "Mana Manager (%)").SetValue(new Slider(30, 1, 100)));

            Menu LaneClearMenu = Menu.AddSubMenu(new Menu("Lane Clear", "LaneClear"));
            LaneClearMenu.AddItem(new MenuItem("laneclearW", "Use W").SetValue(true));
            LaneClearMenu.AddItem(new MenuItem("laneclearE", "Use E").SetValue(true));
            LaneClearMenu.AddItem(new MenuItem("LaneClearManaManager", "Mana Manager (%)").SetValue(new Slider(30, 1, 100)));

            Menu KillStealMenu = Menu.AddSubMenu(new Menu("Kill Steal", "KillSteal"));
            KillStealMenu.AddItem(new MenuItem("KillStealWithAvSpells", "KS with available spells (Q/W/E)").SetValue(true));

            Menu MiscMenu = Menu.AddSubMenu(new Menu("Misc", "Misc"));
            MiscMenu.AddItem(new MenuItem("QAblazedEnemy", "Auto Q if Target's [ABlazed]").SetValue(true));
            MiscMenu.AddItem(new MenuItem("QGapC", "Auto Stun GapClosers").SetValue(true));
            MiscMenu.AddItem(new MenuItem("InterruptEQ", "Auto E-Q to Interrupt").SetValue(false));
            MiscMenu.AddItem(new MenuItem("NearbyREnemies", "Use R in Combo if X Enemies are nearby 'X' ->").SetValue(new Slider(1, 0, 5)));
            
            Menu DrawingMenu = Menu.AddSubMenu(new Menu("Drawing", "Drawing"));
            DrawingMenu.AddItem(new MenuItem("DrawQ", "Draw Q Range").SetValue(true));
            DrawingMenu.AddItem(new MenuItem("DrawPassiveBombOnEnemy", "Draw Passive Bomb on Enemy (Range) (Soon)").SetValue(true));
            DrawingMenu.AddItem(new MenuItem("DrawW", "Draw W Range").SetValue(true));
            DrawingMenu.AddItem(new MenuItem("DrawE", "Draw E Range").SetValue(true));
            DrawingMenu.AddItem(new MenuItem("DrawR", "Draw R Range").SetValue(true));

            Menu.AddToMainMenu();
            #endregion

            #region DrawHPDamage
            var dmgAfterShave = new MenuItem("SurvivorBrand.DrawComboDamage", "Draw Combo Damage").SetValue(true);
            var drawFill =
                new MenuItem("SurvivorBrand.DrawColour", "Fill Color", true).SetValue(
                    new Circle(true, Color.FromArgb(204, 255, 0, 1)));
            Menu.SubMenu("HPBarDrawings").AddItem(drawFill);
            Menu.SubMenu("HPBarDrawings").AddItem(dmgAfterShave);
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
            #endregion

            // Prediction
            SPrediction.Prediction.Initialize();

            #region Subscriptions
            Game.OnUpdate += OnUpdate;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Drawing.OnDraw += OnDraw;
            Game.PrintChat("<font color='#800040'>[SurvivorSeries] Brand</font> <font color='#ff6600'>Loaded.</font>");
            // Add AntiGapCloser + Interrupter + Killsteal //
            #endregion
        }

        private static void OnDraw(EventArgs args)
        {
            //var m = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (Menu.Item("DrawQ").GetValue<bool>())
                Render.Circle.DrawCircle(Player.Position, Q.Range, Color.Aqua);
            //if (Menu.Item("DrawPassiveBombOnEnemy").GetValue<bool>())
            //    Render.Circle.DrawCircle(m.Position, 420f, Color.Orange);
            if (Menu.Item("DrawW").GetValue<bool>())
                Render.Circle.DrawCircle(Player.Position, W.Range, Color.MediumPurple);
            if (Menu.Item("DrawE").GetValue<bool>())
                Render.Circle.DrawCircle(Player.Position, E.Range, Color.LightPink);
            if (Menu.Item("DrawR").GetValue<bool>())
                Render.Circle.DrawCircle(Player.Position, R.Range, Color.MediumVioletRed);
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Player.IsDead || Player.IsRecalling())
                return;

            // Checks
            RManaCost();
            // Combo
            if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.Combo)
            {
                Combo();
            }
            //Lane
            if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.LaneClear) // LaneClear mode broken? kappa
            {
                LaneClear();
                Game.PrintChat("LaneClear Works!");
            }
            if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.Mixed)
            {
                Harass();
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (!Menu.Item("QGapC", true).GetValue<bool>() || Player.Mana < Q.Instance.ManaCost + E.Instance.ManaCost)
                return;

            var t = gapcloser.Sender;

            if (t.IsValidTarget(E.Range) && (t.HasBuff("brandablaze") || E.IsReady()))
            {
                
                E.CastOnUnit(t);
                if (Q.IsReady())
                    Q.Cast(t);
            }
        }

        private static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero t, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!Menu.Item("InterruptEQ", true).GetValue<bool>() || Player.Mana < Q.Instance.ManaCost + E.Instance.ManaCost)
                return;

            if (t.IsValidTarget(E.Range) && (t.HasBuff("brandablaze") || E.IsReady()))
            {
                E.CastOnUnit(t);
                if (Q.IsReady())
                    Q.Cast(t);
            }
        }

        private static float BonusDmg(Obj_AI_Hero target)
        {
            return (float)Player.CalcDamage(target, Damage.DamageType.Magical, (target.MaxHealth * 0.08) - (target.HPRegenRate * 5));
        }

        private static bool LogQUse(Obj_AI_Base m)
        {
            if (m.HasBuff("brandablaze"))
                return true;
            else if (E.Instance.CooldownExpires - Game.Time + 2 >= Q.Instance.Cooldown && W.Instance.CooldownExpires - Game.Time + 2 >= Q.Instance.Cooldown)
                return true;
            else
                return false;
        }

        private static void QUsage()
        {
            var m = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (!m.IsValidTarget())
            {
                return;
            }
            if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.Mixed && Player.ManaPercentage() > Menu.Item("HarassManaManager").GetValue<Slider>().Value)
            {
                if (Menu.Item("harrasQ").GetValue<bool>() && Q.IsInRange(m))
                {
                    Q.CastIfHitchanceEquals(m, HitChance.High);
                }
            }
            // Q Improvement
            if (OktwCommon.GetKsDamage(m, Q) + BonusDmg(m) + OktwCommon.GetEchoLudenDamage(m) > m.Health)
                Q.CastIfHitchanceEquals(m, HitChance.High);

            if (m.HasBuff("brandablaze") && Menu.Item("QAblazedEnemy").GetValue<bool>())
            {
                var spreadTarget = m;

                foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(Q.Range) && enemy.HasBuff("brandablaze")))
                    m = enemy;

                if (spreadTarget == m && !LogQUse(m))
                    return;
            }

            if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.Combo && !W.IsReady() && Player.ManaPercentage() > RManaC + Q.ManaCost)
                Q.CastIfHitchanceEquals(m, HitChance.High);

            if (Player.Mana > RManaC + Q.ManaCost)
            {
                foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(Q.Range)))
                    Q.CastIfHitchanceEquals(m, HitChance.High);
            }
        }

        private static void RManaCost()
        {
            if (!R.IsReady())
            {
                RManaC = W.Instance.ManaCost;
            }
            else
            {
                RManaC = R.Instance.ManaCost;
            }
        }

        private static void WUsage()
        {
            // W Usage
            var t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
            if (t.IsValidTarget())
            {
                if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.Mixed && Player.ManaPercentage() > Menu.Item("HarassManaManager").GetValue<Slider>().Value)
                {
                    if (Menu.Item("harrasW").GetValue<bool>() && W.IsInRange(t))
                    {
                        W.CastIfHitchanceEquals(t, HitChance.VeryHigh);
                    }
                }
                else if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.LaneClear && Player.ManaPercentage() > Menu.Item("LaneClearManaManager").GetValue<Slider>().Value)
                {
                    var allMinions = Cache.GetMinions(Player.ServerPosition, 500, MinionTeam.Enemy);
                    if (Menu.Item("laneclearW").GetValue<bool>() && W.IsReady())
                    {
                        foreach (var minion in allMinions)
                        {
                            if (minion.IsValidTarget())
                            {
                                W.CastIfHitchanceEquals(minion, HitChance.High);
                            }
                        }
                    }
                }
                var Qdamage = Q.GetDamage(t);
                var Wdamage = OktwCommon.GetKsDamage(t, W) + BonusDmg(t);
                if (Wdamage > t.Health)
                {
                    W.CastIfHitchanceEquals(t, HitChance.VeryHigh);
                }
                else if (Wdamage + Qdamage > t.Health && Player.ManaPercentage() > Q.ManaCost + E.ManaCost)
                    W.CastIfHitchanceEquals(t, HitChance.VeryHigh);

                if (Player.Mana > RManaC + W.ManaCost)
                {
                    foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(W.Range)))
                        W.CastIfHitchanceEquals(t, HitChance.VeryHigh);
                }
            }
        }

        private static void EUsage()
        {
            // E Usage
            var t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
            if (t.IsValidTarget())
            {
                // If in Combo Mode
                if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.Combo && Player.Mana > RManaC + E.ManaCost)
                {
                    E.CastOnUnit(t);
                }
                // If In Harass/Mixed Mode
                else if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.Mixed && Player.ManaPercentage() > Menu.Item("HarassManaManager").GetValue<Slider>().Value)
                {
                    if (Menu.Item("harrasE").GetValue<bool>() && E.IsInRange(t))
                    {
                        E.CastOnUnit(t);
                    }
                }
                else if (Orbwalker.ActiveMode == SebbyLib.Orbwalking.OrbwalkingMode.LaneClear && Player.ManaPercentage() > Menu.Item("LaneClearManaManager").GetValue<Slider>().Value)
                {
                    var allMinions = Cache.GetMinions(Player.ServerPosition, 500, MinionTeam.Enemy);
                    if (Menu.Item("laneclearE").GetValue<bool>() && E.IsReady())
                    {
                        foreach (var minion in allMinions)
                        {
                            if (minion.IsValidTarget())
                            {
                                E.CastOnUnit(minion);
                            }
                        }
                    }
                }
                else
                {
                    // If there's a chance to KS/Get :kappa: a kill.
                    var eDmg = OktwCommon.GetKsDamage(t, E) + BonusDmg(t) + OktwCommon.GetEchoLudenDamage(t);
                    var wDmg = W.GetDamage(t);
                    if (eDmg > t.Health)
                    {
                        E.CastOnUnit(t);
                    }
                    else if (wDmg + eDmg > t.Health && Player.Mana > W.ManaCost + E.ManaCost)
                    {
                        E.CastOnUnit(t);
                        W.CastIfHitchanceEquals(t, HitChance.VeryHigh);
                    }
                }
            }
        }

        private static void RUsage()
        {
            // Massive Thanks to Sebby for the time saved instead of writing it from scratch, instead I 'stole' it from him :feelsbadman:
            var bounceRange = 430;
            var t2 = TargetSelector.GetTarget(R.Range + bounceRange, TargetSelector.DamageType.Magical);

            if (t2.IsValidTarget(R.Range) && t2.CountEnemiesInRange(bounceRange) >= Menu.Item("NearbyREnemies").GetValue<Slider>().Value && Menu.Item("NearbyREnemies").GetValue<Slider>().Value > 0)
                R.Cast(t2);

            if (t2.IsValidTarget() && OktwCommon.ValidUlt(t2))
            {
                if (t2.CountAlliesInRange(550) == 0 || Player.HealthPercent < 50 || t2.CountEnemiesInRange(bounceRange) > 1)
                {
                    var prepos = R.GetPrediction(t2).CastPosition;
                    var dmgR = R.GetDamage(t2);

                    if (t2.Health < dmgR * 3)
                    {
                        var totalDmg = dmgR;
                        var minionCount = CountMinionsInRange(bounceRange, prepos);

                        if (t2.IsValidTarget(R.Range))
                        {
                            if (prepos.CountEnemiesInRange(bounceRange) > 1)
                            {
                                if (minionCount > 2)
                                    totalDmg = dmgR * 2;
                                else
                                    totalDmg = dmgR * 3;
                            }
                            else if (minionCount > 0)
                            {
                                totalDmg = dmgR * 2;
                            }

                            if (W.IsReady())
                            {
                                totalDmg += W.GetDamage(t2);
                            }

                            if (E.IsReady())
                            {
                                totalDmg += E.GetDamage(t2);
                            }

                            if (Q.IsReady())
                            {
                                totalDmg += Q.GetDamage(t2);
                            }

                            totalDmg += BonusDmg(t2);
                            totalDmg += OktwCommon.GetEchoLudenDamage(t2);

                            // Hex
                            if (Items.HasItem(3155, t2))
                            {
                                // Maximum is 280, but lets use 250 as an average value.
                                totalDmg = totalDmg - 250;
                            }

                            // MoM
                            if (Items.HasItem(3156, t2))
                            {
                                // Nerfed? Kappa
                                totalDmg = totalDmg - 300;
                            }

                            if (totalDmg > t2.Health - OktwCommon.GetIncomingDamage(t2) && Player.GetAutoAttackDamage(t2) * 2 < t2.Health)
                            {
                                // Kill the Target :feelsbadman:
                                R.CastOnUnit(t2);
                            }

                        }
                        else if (t2.Health - OktwCommon.GetIncomingDamage(t2) < dmgR * 2 + BonusDmg(t2))
                        {
                            if (Player.CountEnemiesInRange(R.Range) > 0)
                            {
                                foreach (var t in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(R.Range) && enemy.Distance(prepos) < bounceRange))
                                {
                                    R.CastOnUnit(t);
                                }
                            }
                            else
                            {
                                var minions = Cache.GetMinions(Player.Position, R.Range);
                                foreach (var minion in minions.Where(minion => minion.IsValidTarget(R.Range) && minion.Distance(prepos) < bounceRange))
                                {
                                    R.CastOnUnit(minion);
                                }
                            }
                        }
                    }
                }
            }
        }

        private double Wdmg(Obj_AI_Base target)
        {
            return target.MaxHealth * (4.5 + W.Level * 1.5) * 0.01;
        }

        private static int CountMinionsInRange(float range, Vector3 pos)
        {
            var minions = Cache.GetMinions(pos, range);
            int count = 0;
            foreach (var minion in minions)
            {
                count++;
            }
            return count;
        }

        private static void Combo()
        {
            // If Mana is < ManaManager : return;
            // Combo
            WUsage();
            QUsage();
            EUsage();
            RUsage();
        }

        private static void Harass()
        {
            // Harass
            WUsage();
            QUsage();
            EUsage();
        }

        private static void LaneClear()
        {
            // LaneClear
            WUsage();
            EUsage();
        }

        private static float CalculateDamage(Obj_AI_Base enemy)
        {
            // Calculate Damage + Bleed ticking
            float damage = 0;
            float BrandPassive = (enemy.MaxHealth * 14) / 100;
            float PassiveDamage = (enemy.MaxHealth * 2) / 100;
            // bool HasABlazeBomb = enemy.HasBuff("brandablazebomb");
            damage += BrandPassive;
            if (enemy.HasBuff("brandablazebomb"))
            {
                damage += PassiveDamage;
            }
            double ultdamage = 0;

            if (Q.IsReady())
            {
                damage += Q.GetDamage(enemy);
            }

            if (W.IsReady())
            {
                if (enemy.HasBuff("brandablaze") || enemy.HasBuff("ablaze"))
                {
                    float BonusWDmg = W.GetDamage(enemy) + ((W.GetDamage(enemy) * 25) / 100);
                    damage += BonusWDmg;
                }
                else
                {
                    damage += W.GetDamage(enemy);
                }
            }

            if (E.IsReady())
            {
                damage += E.GetDamage(enemy);
            }

            if (R.IsReady())
            {
                ultdamage += Player.GetSpellDamage(enemy, SpellSlot.R);
            }

            return damage;
        }

    }
}