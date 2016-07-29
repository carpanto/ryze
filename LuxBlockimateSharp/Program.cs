using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace LuxBlockimateSharp
{
    class Program
    {

        #region Declaration
        private static Spell NocR, KhaR, ShacoQ;
        private static Menu Menu;
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static string Nocturne = "Nocturne";
        private static string KhaZix = "Kha'Zix";
        private static string Shaco = "Shaco";
        #endregion

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

            #region Spells
            if (ChampionNoc)
            {
                NocR = new Spell(SpellSlot.R);
            }
            if (ChampionKha)
            {
                KhaR = new Spell(SpellSlot.R);
            }
            if (ChampionShaco)
            {
                ShacoQ = new Spell(SpellSlot.Q);
            }
            #endregion

            #region Menu
            Menu = new Menu("LuxBlockimate#", "LuxBlockimateSharp", true);

            Menu Spells = Menu.AddSubMenu(new Menu("Spells", "Spells"));
            if (ChampionShaco)
            {
                Spells.AddItem(new MenuItem("BlockShaco", "Exploit with Q").SetValue(true).SetTooltip("Will dodge Lux ultimate with Q (Exploit)"));
            }
            if (ChampionNoc)
            {
                Spells.AddItem(new MenuItem("BlockNoc", "Exploit with R").SetValue(true).SetTooltip("Will dodge Lux ultimate with R (Exploit)"));
            }
            if (ChampionKha)
            {
                Spells.AddItem(new MenuItem("BlockKha", "Exploit with R").SetValue(true).SetTooltip("Will dodge Lux ultimate with R (Exploit)"));
            }

            // Menu DrawingMenu = Menu.AddSubMenu(new Menu("Drawing", "Drawing"));
            // Lux Ult?

            Menu.AddToMainMenu();
            #endregion

            #region Subscriptions
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpell;
            #endregion
        }

        public static void Obj_AI_Base_OnProcessSpell(Obj_AI_Base enemy, GameObjectProcessSpellCastEventArgs Spell)
        {
            var champpos = Player.ServerPosition;
            //Game.PrintChat("-1");
            //Console.WriteLine("Test 0");
            if (enemy.IsMe)
                return;
            if (enemy.IsChampion())
            {
                //Game.PrintChat("DEBUG: Before (if Spell.SData) Check");
                //var luxr = enemy.Spellbook.GetSpell(SpellSlot.R);
                if (Spell.SData.Name.ToLower() == "luxmalicecannon" || Spell.SData.Name.ToLower() == "lux malice cannon" || Spell.SData.Name.ToLower() == "luxmalicecannonmis" || Spell.SData.Name.ToLower() == "lux malice cannon mis" || Spell.SData.DisplayName.ToLower() == "finalspark")
                {
                    //Game.PrintChat("DEBUG: After (if Spell.SData) Check");
                    //if (champpos.ProjectOn(Spell.Start.To2D(), Spell.End.To2D()).IsOnSegment)
                    //var startPos = enemy.ServerPosition; //get lux
                    //var endPos = Player.ServerPosition.Extend(Spell.Start, );

                    var rectangle = new Geometry.Polygon.Rectangle(Spell.Start, Spell.End, 50);
                    if (rectangle.IsInside(Player))
                    {
                        //Game.PrintChat("DEBUG: Inside of ShacoQ Casting");
                        if (Player.ChampionName == Shaco)
                        {
                            ShacoQ.Cast(Player.Position);
                        }
                        if (Player.ChampionName == Nocturne)
                        {
                            NocR.Cast();
                        }
                        if (Player.ChampionName == KhaZix)
                        {
                            KhaR.Cast();
                        }
                    }
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
    }
}