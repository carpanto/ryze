// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="LuxBlockimateSharp">
//      Copyright (c) LuxBlockimateSharp. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace LuxBlockimateSharp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            //if (Player.ChampionName != Nocturne || Player.ChampionName != KhaZix || Player.ChampionName != Shaco)
            //return;

            var ChampionNoc = Player.ChampionName == Nocturne;
            var ChampionKha = Player.ChampionName == KhaZix;
            var ChampionShaco = Player.ChampionName == Shaco;
            var ChampionGraves = Player.ChampionName == Graves;

            #region Spells

            if (ChampionNoc)
                NocR = new Spell(SpellSlot.R);
            if (ChampionKha)
                KhaR = new Spell(SpellSlot.R);
            if (ChampionShaco)
                ShacoQ = new Spell(SpellSlot.Q);
            if (ChampionGraves)
                GravesW = new Spell(SpellSlot.W);

            #endregion

            #region Menu

            Menu = new Menu("LuxBlockimate#", "LuxBlockimateSharp", true);

            var Spells = Menu.AddSubMenu(new Menu("Spells", "Spells"));
            if (ChampionShaco)
                Spells.AddItem(
                    new MenuItem("BlockShaco", "Exploit with Q").SetValue(true)
                        .SetTooltip("Will dodge Lux ultimate with Q (Exploit)"));
            if (ChampionNoc)
            {
                Spells.AddItem(
                    new MenuItem("BlockNoc", "Exploit with R").SetValue(true)
                        .SetTooltip("Will dodge Lux ultimate with R (Exploit)"));
                Spells.AddItem(new MenuItem("ProtectAllyNoc", "Protect Ally by using Exploit with R").SetValue(true));
            }
            if (ChampionKha)
                Spells.AddItem(
                    new MenuItem("BlockKha", "Exploit with R").SetValue(true)
                        .SetTooltip("Will dodge Lux ultimate with R (Exploit)"));
            if (ChampionGraves)
            {
                Spells.AddItem(
                    new MenuItem("BlockGraves", "Exploit with W").SetValue(true)
                        .SetTooltip("Will dodge Lux ultimate with W (Exploit)"));
                Spells.AddItem(new MenuItem("ProtectAllyGraves", "Protect Ally by using Exploit with W").SetValue(true));
            }

            // Menu DrawingMenu = Menu.AddSubMenu(new Menu("Drawing", "Drawing"));
            // Lux Ult?

            Menu.AddToMainMenu();

            #endregion

            #region Subscriptions

            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpell;

            #endregion

            Game.PrintChat(
                "<font color='#800040'>[Exploit] LuxBlockimateSharp</font> <font color='#ff6600'>Loaded.</font>");
        }

        public static void Obj_AI_Base_OnProcessSpell(Obj_AI_Base enemy, GameObjectProcessSpellCastEventArgs Spell)
        {
            if (enemy.IsMe)
                return;
            if (enemy.IsChampion() && enemy.IsEnemy)
                if ((Spell.SData.Name.ToLower() == "luxmalicecannon") ||
                    (Spell.SData.Name.ToLower() == "lux malice cannon") ||
                    (Spell.SData.Name.ToLower() == "luxmalicecannonmis") ||
                    (Spell.SData.Name.ToLower() == "lux malice cannon mis") ||
                    (Spell.SData.DisplayName.ToLower() == "finalspark"))
                {
                    var rectangle = new Geometry.Polygon.Rectangle(Spell.Start, Spell.End, 50);
                    if (rectangle.IsInside(Player))
                    {
                        if (Player.ChampionName == Shaco)
                            ShacoQ.Cast(Player.Position);
                        if (Player.ChampionName == Nocturne)
                            NocR.Cast();
                        if ((Player.ChampionName == Graves) && enemy.IsValidTarget(950f))
                            GravesW.Cast(enemy.Position);
                        if (Player.ChampionName == KhaZix)
                            KhaR.Cast();
                    }
                    foreach (var ally in HeroManager.Allies.Where(ally => rectangle.IsInside(ally)))
                    {
                        if ((Player.ChampionName == Graves) && Menu.Item("ProtectAllyGraves").GetValue<bool>() &&
                            enemy.IsValidTarget(950f))
                            GravesW.Cast(enemy.Position);
                        if ((Player.ChampionName == Nocturne) && Menu.Item("ProtectAllyNoc").GetValue<bool>())
                            NocR.Cast();
                    }
                }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead)
                return;

            //
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            // Draw Lux Ult?
        }

        #region Declaration

        private static Spell NocR, KhaR, ShacoQ, GravesW;
        private static Menu Menu;

        private static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        private static readonly string Nocturne = "Nocturne";
        private static readonly string KhaZix = "Kha'Zix";
        private static readonly string Shaco = "Shaco";
        private static readonly string Graves = "Graves";

        #endregion
    }
}