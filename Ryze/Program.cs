/**
 * 
 * Love Ya Lads!
 * 
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
using SharpDX;
using SebbyLib;
using Color = System.Drawing.Color;

namespace SurvivorRyze
{
    class Program
    {

        #region Declaration
        private static Spell Q, W, E, R;
        private static SpellSlot IgniteSlot;
        private static Items.Item HealthPot;
        private static Items.Item BiscuitOfRej;
        private static Items.Item TearOfGod;
        private static Items.Item Seraph;
        private static Items.Item Manamune;
        private static Items.Item Archangel;
        private static Items.Item Flask;
        private static Items.Item HexProtobelt;
        private static Items.Item HexGunBlade;
        private static Items.Item HexGLP;
        private static SebbyLib.Orbwalking.Orbwalker Orbwalker;
        private static Menu Menu;
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private const string ChampionName = "Ryze";
        #endregion

        public static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != ChampionName)
                return;

            #region Spells
            Q = new Spell(SpellSlot.Q, 1000f);
            Q.SetSkillshot(0.7f, 55f, float.MaxValue, true, SkillshotType.SkillshotLine);
            W = new Spell(SpellSlot.W, 610f);
            W.SetTargetted(0.103f, 550f);
            E = new Spell(SpellSlot.E, 610f);
            E.SetTargetted(.5f, 550f);
            R = new Spell(SpellSlot.R, 1500f);
            R.SetTargetted(2f, 1500f);
            #endregion

            #region Items/SummonerSpells
            IgniteSlot = Player.GetSpellSlot("summonerdot");
            HealthPot = new Items.Item(2003, 0);
            BiscuitOfRej = new Items.Item(2010, 0);
            TearOfGod = new Items.Item(3070, 0);
            Manamune = new Items.Item(3004, 0);
            Seraph = new Items.Item(3040, 0);
            Archangel = new Items.Item(3003, 0);
            Flask = new Items.Item(2041, 0);
            HexGunBlade = new Items.Item(3146, 600f);
            HexProtobelt = new Items.Item(3152, 300f);
            HexGLP = new Items.Item(3030, 300f);
            #endregion

            #region Menu
            Menu = new Menu("SurvivorRyze", "SurvivorRyze", true);

            Menu OrbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new SebbyLib.Orbwalking.Orbwalker(OrbwalkerMenu);

            Menu TargetSelectorMenu = Menu.AddSubMenu(new Menu("Target Selector", "TargetSelector"));
            TargetSelector.AddToMenu(TargetSelectorMenu);

            Menu ComboMenu = Menu.AddSubMenu(new Menu("Combo", "Combo"));
            ComboMenu.AddItem(new MenuItem("CUseQ", "Cast Q").SetValue(true));
            ComboMenu.AddItem(new MenuItem("CUseW", "Cast W").SetValue(true));
            ComboMenu.AddItem(new MenuItem("CUseE", "Cast E").SetValue(true));
            ComboMenu.AddItem(new MenuItem("CBlockAA", "Block AA in Combo Mode").SetValue(true));
            ComboMenu.AddItem(new MenuItem("Combo2TimesMana", "Champion needs to have mana for atleast 2 times (Q/W/E)?").SetValue(true).SetTooltip("If it's set to 'false' it'll need atleast mana for Q/W/E [1x] Post in thread if needs a change"));
            ComboMenu.AddItem(new MenuItem("CUseR", "Ultimate (R) in Misc Menu"));
            ComboMenu.AddItem(new MenuItem("CUseIgnite", "Use Ignite (Smart)").SetValue(true));

            Menu HarassMenu = Menu.AddSubMenu(new Menu("Harass", "Harass"));
            HarassMenu.AddItem(new MenuItem("HarassQ", "Use Q").SetValue(true));
            HarassMenu.AddItem(new MenuItem("HarassW", "Use W").SetValue(false));
            HarassMenu.AddItem(new MenuItem("HarassE", "Use E").SetValue(false));
            HarassMenu.AddItem(new MenuItem("HarassManaManager", "Mana Manager (%)").SetValue(new Slider(30, 1, 100)));

            Menu LaneClearMenu = Menu.AddSubMenu(new Menu("Lane Clear", "LaneClear"));
            LaneClearMenu.AddItem(new MenuItem("UseQLC", "Use Q to LaneClear").SetValue(true));
            LaneClearMenu.AddItem(new MenuItem("UseELC", "Use E to LaneClear").SetValue(true));
            LaneClearMenu.AddItem(new MenuItem("LaneClearManaManager", "Mana Manager (%)").SetValue(new Slider(30, 1, 100)));

            Menu ItemsMenu = Menu.AddSubMenu(new Menu("Items Menu", "ItemsMenu"));
            ItemsMenu.AddItem(new MenuItem("UseHPPotion", "Use HP Potion/Biscuit/Flask at % Health").SetValue(new Slider(15, 0, 100)));
            ItemsMenu.AddItem(new MenuItem("UseItemFlask", "Use Flasks If You don't have Potions?").SetValue(true));
            ItemsMenu.AddItem(new MenuItem("UsePotionOnlyIfEnemiesAreInRange", "Use Potions/Flasks only if Enemies are nearby?").SetValue(true).SetTooltip("It'll use the potions/flask if enemies are within the e.g (~R Range)"));
            ItemsMenu.AddItem(new MenuItem("UseSeraph", "Use [Seraph's Embrace]?").SetValue(true));
            ItemsMenu.AddItem(new MenuItem("UseSeraphIfEnemiesAreNearby", "Use [Seraph's Embrace] only if Enemies are nearby?").SetValue(true));
            ItemsMenu.AddItem(new MenuItem("UseSeraphAtHP", "Activate [Seraph's Embrace] at HP %?").SetValue(new Slider(15, 0, 100)));
            ItemsMenu.AddItem(new MenuItem("UseHexGunBlade", "Use [Hextech Gunblade]?").SetValue(true));
            ItemsMenu.AddItem(new MenuItem("UseHexProtobelt", "Use [Hextech Protobelt-01]?").SetValue(true));
            ItemsMenu.AddItem(new MenuItem("UseHexGLP", "Use [Hextech GLP-800]?").SetValue(true));
            ItemsMenu.AddItem(new MenuItem("HexGunBladeAtHP", "Use [Hextech Gunblade] at HP %?").SetValue(new Slider(25, 0, 100)));
            ItemsMenu.AddItem(new MenuItem("HexProtobeltAtHP", "Use [Hextech Protobelt-01] at HP %?").SetValue(new Slider(25, 0, 100)));
            ItemsMenu.AddItem(new MenuItem("HexGLPAtHP", "Use [Hextech GLP-800] at HP %?").SetValue(new Slider(25, 0, 100)));
            ItemsMenu.AddItem(new MenuItem("StackTear", "Stack Tear/Manamune/Archangel in Fountain?").SetValue(true).SetTooltip("Stack it in Fountain?"));
            ItemsMenu.AddItem(new MenuItem("StackTearNF", "Stack Tear/Manamune/Archangel if You've Blue Buff?").SetValue(true));

            Menu MiscMenu = Menu.AddSubMenu(new Menu("Misc Menu", "MiscMenu"));
            MiscMenu.AddItem(new MenuItem("KSQ", "Use Q to KS").SetValue(true));
            MiscMenu.AddItem(new MenuItem("KSW", "Use W to KS").SetValue(true));
            MiscMenu.AddItem(new MenuItem("KSE", "Use E to KS").SetValue(true));
            MiscMenu.AddItem(new MenuItem("InterruptWithW", "Use W to Interrupt Channeling Spells").SetValue(true));
            MiscMenu.AddItem(new MenuItem("WGapCloser", "Use W on Enemy GapCloser (Irelia's Q)").SetValue(true));
            MiscMenu.AddItem(new MenuItem("ChaseWithR", "Use R to Chase (Being Added)"));
            MiscMenu.AddItem(new MenuItem("EscapeWithR", "Use R to Escape (Being Added)"));
            
            Menu DrawingMenu = Menu.AddSubMenu(new Menu("Drawing", "Drawing"));
            DrawingMenu.AddItem(new MenuItem("DrawQ", "Draw Q Range").SetValue(true));
            DrawingMenu.AddItem(new MenuItem("DrawWE", "Draw W/E Range").SetValue(true));
            DrawingMenu.AddItem(new MenuItem("DrawR", "Draw R Range").SetValue(true));

            Menu.AddToMainMenu();
            #endregion

            #region DrawHPDamage
            var dmgAfterShave = new MenuItem("SurvivorRyze.DrawComboDamage", "Draw Combo Damage").SetValue(true);
            var drawFill =
                new MenuItem("SurvivorRyze.DrawColour", "Fill Color", true).SetValue(
                    new Circle(true, Color.SeaGreen));
            DrawingMenu.AddItem(drawFill);
            DrawingMenu.AddItem(dmgAfterShave);
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

            #region Subscriptions
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            #endregion
            Game.PrintChat("<font color='#800040'>[SurvivorSeries] Ryze</font> <font color='#ff6600'>Loaded.</font>");
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Menu.Item("DrawQ").GetValue<bool>())
                Render.Circle.DrawCircle(Player.Position, Q.Range, Color.Aqua);
            if (Menu.Item("DrawWE").GetValue<bool>())
                Render.Circle.DrawCircle(Player.Position, W.Range, Color.AliceBlue);
            if (Menu.Item("DrawR").GetValue<bool>())
                Render.Circle.DrawCircle(Player.Position, R.Range, Color.Orchid);
        }

        private static void AABlock()
        {
            Orbwalker.SetAttack(!Menu.Item("CBlockAA").GetValue<bool>());
        }

        private static void ItemsChecks()
        {
            // Check when you can use items (potions, ex) && Cast them (Probelt Usage please)
            if (Player.HealthPercentage() < Menu.Item("UseHPPotion").GetValue<Slider>().Value && !Player.InFountain() || !Player.IsRecalling())
            {
                if (Player.CountEnemiesInRange(1250) > 0 && Menu.Item("UsePotionOnlyIfEnemiesAreInRange").GetValue<bool>())
                {
                    if (!Player.HasBuff("RegenerationPotion") || !Player.HasBuff("FlaskOfCrystalWater") || !Player.HasBuff("ItemCrystalFlask") && Items.HasItem(2003))
                    {
                        HealthPot.Cast();
                    }
                    if (!Player.HasBuff("FlaskOfCrystalWater") || !Player.HasBuff("ItemCrystalFlask") || !Player.HasBuff("ItemMiniRegenPotion") && Items.HasItem(2010))
                    {
                        BiscuitOfRej.Cast();
                    }
                    if (Menu.Item("UseItemFlask").GetValue<bool>() && !Player.HasBuff("FlaskOfCrystalWater") || !Player.HasBuff("ItemCrystalFlask") || !Player.HasBuff("RegenerationPotion") || !Player.HasBuff("ItemMiniRegenPotion") && Items.HasItem(2041))
                    {
                        Flask.Cast();
                    }
                }
            }
            if (Player.HealthPercentage() < Menu.Item("UseSeraphAtHP").GetValue<Slider>().Value && Menu.Item("UseSeraph").GetValue<bool>() && !Player.InFountain() || !Player.IsRecalling())
            {
                if (Player.CountEnemiesInRange(1000) > 0 && Menu.Item("UseSeraphIfEnemiesAreNearby").GetValue<bool>())
                {
                    if (Seraph.IsReady() && Seraph.IsOwned(Player))
                    {
                        Seraph.Cast();
                    }
                }
            }
            var target = TargetSelector.GetTarget(600f, TargetSelector.DamageType.Magical);

            // If Target's not in Q Range or there's no target or target's invulnerable don't fuck with him
            if (target == null || !target.IsValidTarget(600f) || target.IsInvulnerable)
                return;

            if (Menu.Item("UseHexGunBlade").GetValue<bool>() && target.IsValidTarget(600) && target.HealthPercentage() < Menu.Item("HexGunBladeAtHP").GetValue<Slider>().Value)
            {
                Items.UseItem(3146, target);
            }
            if (Menu.Item("UseHexProtobelt").GetValue<bool>() && target.IsValidTarget(300) && target.HealthPercentage() < Menu.Item("HexProtobeltAtHP").GetValue<Slider>().Value)
            {
                Items.UseItem(3152, target.Position);
            }
            if (Menu.Item("UseHexGLP").GetValue<bool>() && target.IsValidTarget(300) && target.HealthPercentage() < Menu.Item("HexGLPAtHP").GetValue<Slider>().Value)
            {
                Items.UseItem(3030, target.Position);
            }
        }

        private static void StackItems()
        {
            if (Player.InFountain() || Player.HasBuff("CrestoftheAncientGolem") && Menu.Item("StackTearNF").GetValue<bool>()) // Add if Player has Blue Buff
            {
                if (Items.HasItem(3004, Player) || Items.HasItem(3003, Player) || Items.HasItem(3070, Player) || Items.HasItem(3072, Player) || Items.HasItem(3073, Player) || Items.HasItem(3008, Player))
                {
                    Q.Cast(Player.ServerPosition);
                }
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead || Player.IsRecalling())
                return;

            Orbwalker.SetAttack(true);
            if (Menu.Item("StackTear").GetValue<bool>())
            {
                StackItems();
            }
            ItemsChecks();
            KSCheck();
            switch (Orbwalker.ActiveMode)
            {
                case SebbyLib.Orbwalking.OrbwalkingMode.Combo:
                        Combo();
                    break;
                case SebbyLib.Orbwalking.OrbwalkingMode.Mixed:
                        Harass();
                    break;
                case SebbyLib.Orbwalking.OrbwalkingMode.LaneClear:
                        LaneClear();
                    break;
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (!Menu.Item("WGapCloser").GetValue<bool>() || Player.Mana < W.Instance.ManaCost + Q.Instance.ManaCost)
                return;

            var t = gapcloser.Sender;

            if (gapcloser.End.Distance(Player.ServerPosition) < W.Range)
            {
                W.Cast(t);
            }
        }

        private static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero t, Interrupter2.InterruptableTargetEventArgs args)
        {
            var WCast = Menu.Item("InterruptWithW").GetValue<bool>();
            if (!WCast || !t.IsValidTarget(W.Range) || !W.IsReady()) return;
            W.Cast(t);
        }

        private static void KSCheck()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);

            // If Target's not in Q Range or there's no target or target's invulnerable don't fuck with him
            if (target == null || !target.IsValidTarget(Q.Range) || target.IsInvulnerable)
                return;

            var ksQ = Menu.Item("KSQ").GetValue<bool>();
            var ksW = Menu.Item("KSW").GetValue<bool>();
            var ksE = Menu.Item("KSE").GetValue<bool>();

            var predpos = Q.GetPrediction(target);

            // KS
            if (ksQ && Q.GetDamage(target) > target.Health && target.IsValidTarget(Q.Range))
            {
                if (target.CanMove && predpos.Hitchance >= HitChance.High)
                {
                    Q.Cast(predpos.CastPosition);
                }
                else if (!target.CanMove)
                {
                    Q.Cast(target);
                }
            }
            if (ksW && W.GetDamage(target) > target.Health && target.IsValidTarget(W.Range))
            {
                W.CastOnUnit(target);
            }
            if (ksE && W.GetDamage(target) > target.Health && target.IsValidTarget(E.Range))
            {
                E.CastOnUnit(target);
            }
        }

        private static void Combo()
        {
            // Combo
            var CUseQ = Menu.Item("CUseQ").GetValue<bool>();
            var CUseW = Menu.Item("CUseW").GetValue<bool>();
            var CUseE = Menu.Item("CUseE").GetValue<bool>();
            // Checks
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            var predpos = Q.GetPrediction(target);

            // If Target's not in Q Range or there's no target or target's invulnerable don't fuck with him
            if (target == null || !target.IsValidTarget(Q.Range) || target.IsInvulnerable)
                return;
            // Execute the Lad
            if (Menu.Item("CUseIgnite").GetValue<bool>() && target.Health < Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite))
            {
                Player.Spellbook.CastSpell(IgniteSlot, target);
            }
            if (Menu.Item("Combo2TimesMana").GetValue<bool>())
            {
                if (Player.Mana >= 2 * (Q.Instance.ManaCost + W.Instance.ManaCost + E.Instance.ManaCost))
                {
                    if (CUseQ && CUseW && CUseE && target.IsValidTarget(Q.Range))
                    {
                        if (target.CanMove && predpos.Hitchance >= HitChance.High)
                        {
                            Q.Cast(predpos.CastPosition);
                        }
                        else if (!target.CanMove)
                        {
                            Q.Cast(target);
                        }
                        if (target.IsValidTarget(W.Range) && W.IsReady())
                        {
                            W.CastOnUnit(target);
                        }
                        if (target.IsValidTarget(E.Range) && E.IsReady())
                        {
                            E.CastOnUnit(target);
                        }
                    }
                    if (CUseW && target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.CastOnUnit(target);
                    }
                    if (CUseQ && target.IsValidTarget(Q.Range))
                    {
                        if (target.CanMove && predpos.Hitchance >= HitChance.High)
                        {
                            Q.Cast(predpos.CastPosition);
                        }
                        else if (!target.CanMove)
                        {
                            Q.Cast(target);
                        }
                    }
                    if (CUseE && target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.CastOnUnit(target);
                    }
                }
            }
            else
            {
                if (Player.Mana >= Q.Instance.ManaCost + W.Instance.ManaCost + E.Instance.ManaCost)
                {
                    if (CUseW && target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.CastOnUnit(target);
                    }
                    if (CUseQ && target.IsValidTarget(Q.Range))
                    {
                        if (target.CanMove && predpos.Hitchance >= HitChance.High)
                        {
                            Q.Cast(predpos.CastPosition);
                        }
                        else if (!target.CanMove)
                        {
                            Q.Cast(target);
                        }
                    }
                    if (CUseE && target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.CastOnUnit(target);
                    }
                }
                else
                {
                    if (CUseW && target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.CastOnUnit(target);
                    }
                    if (CUseQ && target.IsValidTarget(Q.Range))
                    {
                        if (target.CanMove && predpos.Hitchance >= HitChance.High)
                        {
                            Q.Cast(predpos.CastPosition);
                        }
                        else if (!target.CanMove)
                        {
                            Q.Cast(target);
                        }
                    }
                    if (CUseE && target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.CastOnUnit(target);
                    }
                }
            }
        }

        private static void Harass()
        {
            // Harass
            var HarassUseQ = Menu.Item("HarassQ").GetValue<bool>();
            var HarassUseW = Menu.Item("HarassW").GetValue<bool>();
            var HarassUseE = Menu.Item("HarassE").GetValue<bool>();
            // Checks
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            var predpos = Q.GetPrediction(target);

            // If Target's not in Q Range or there's no target or target's invulnerable don't fuck with him
            if (target == null || !target.IsValidTarget(Q.Range) || target.IsInvulnerable)
                return;
            // Execute the Lad
            if (Player.ManaPercentage() > Menu.Item("HarassManaManager").GetValue<Slider>().Value)
            {
                if (HarassUseW && target.IsValidTarget(W.Range))
                {
                    W.CastOnUnit(target);
                }
                if (HarassUseQ && target.IsValidTarget(Q.Range))
                {
                    if (target.CanMove && predpos.Hitchance >= HitChance.High)
                    {
                        Q.Cast(predpos.CastPosition);
                    }
                    else if (!target.CanMove)
                    {
                        Q.Cast(target);
                    }
                }
                if (HarassUseE && target.IsValidTarget(W.Range))
                {
                    E.CastOnUnit(target);
                }
            }
        }

        private static void LaneClear()
        {
            // LaneClear
            if (Menu.Item("UseQLC").GetValue<bool>() || Menu.Item("UseELC").GetValue<bool>())
            {
                if (Player.ManaPercentage() > Menu.Item("LaneClearManaManager").GetValue<Slider>().Value)
                {
                    var allMinions = Cache.GetMinions(Player.ServerPosition, E.Range, MinionTeam.Enemy);
                    if (!Q.IsReady() && E.IsReady())
                    {
                        foreach (var minion in allMinions)
                        {
                            if (minion.IsValidTarget(E.Range) && minion.HasBuff("RyzeE") && minion.Health < E.GetDamage(minion))
                            {
                                E.CastOnUnit(minion);
                            }
                        }
                    }
                    if (Q.IsReady() && E.IsReady())
                    {
                        foreach (var minion in allMinions)
                        {
                            if (minion.IsValidTarget(Q.Range) && minion.HasBuff("RyzeE") && Q.GetDamage(minion) + E.GetDamage(minion) > minion.Health)
                            {
                                Q.Cast(minion);
                                if (minion.IsValidTarget(E.Range))
                                {
                                    E.CastOnUnit(minion);
                                }
                            }
                            else if (minion.IsValidTarget(Q.Range) && !minion.HasBuff("RyzeE") && minion.Health < Q.GetDamage(minion) + E.GetDamage(minion))
                            {
                                if (minion.IsValidTarget(E.Range))
                                {
                                    E.CastOnUnit(minion);
                                }
                                Q.Cast(minion);
                            }
                        }
                    }
                }
            }
        } // LaneClear End

        private static float CalculateDamage(Obj_AI_Base enemy)
        {
            float damage = 0;
            if (Q.IsReady() || Player.Mana <= Q.Instance.ManaCost + Q.Instance.ManaCost)
                damage += Q.GetDamage(enemy) + Q.GetDamage(enemy);
            else if (Q.IsReady() || Player.Mana <= Q.Instance.ManaCost)
                damage += Q.GetDamage(enemy);

            if (W.IsReady() || Player.Mana <= W.Instance.ManaCost + W.Instance.ManaCost)
                damage += W.GetDamage(enemy) + W.GetDamage(enemy);
            else if (W.IsReady() || Player.Mana <= W.Instance.ManaCost)
                damage += W.GetDamage(enemy);

            if (E.IsReady() || Player.Mana <= E.Instance.ManaCost + E.Instance.ManaCost)
                damage += E.GetDamage(enemy) + E.GetDamage(enemy);
            else if (E.IsReady() || Player.Mana <= E.Instance.ManaCost)
                damage += E.GetDamage(enemy);

            if (Menu.Item("CUseIgnite").GetValue<bool>())
            {
                damage += (float)Player.GetSummonerSpellDamage(enemy, Damage.SummonerSpell.Ignite);
            }

            return damage;
        }
    }
}