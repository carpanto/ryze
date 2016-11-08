using System;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
using Color = SharpDX.Color;

namespace Dev_Essentials
{
    internal class Program
    {
        private static Spell Q, W, E, R;


        public static Menu Config;
        public static Obj_AI_Hero Player = ObjectManager.Player;


        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
            Drawing.OnDraw += OnDraw;
        }


        private static void OnGameLoad(EventArgs args)
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R);

            Notifications.AddNotification("DevPrivate by eXtraGoZ Loaded", 3000);
            Config = new Menu("DevEssentials", "DevPrivate", true).SetFontStyle(FontStyle.Bold, Color.Red);
            Config.SubMenu("DevEssentials")
                .AddItem(
                    new MenuItem("ActiveConsole", "Write to Console").SetValue(new KeyBind("G".ToCharArray()[0],
                        KeyBindType.Press)));
            Config.SubMenu("DevEssentials").AddItem(new MenuItem("Active", "Active Dev-Mode?").SetValue(true));
            Game.OnUpdate += OnGameUpdate;
        }


        private static void OnGameUpdate(EventArgs args)
        {
            if (!Config.Item("Active").GetValue<bool>())
                return;
            var PlayerCrit = Player.Crit.ToString();
            var PlayerCrit1 = PlayerCrit.Replace("0,", "") + "%";

            var temp = "";
            var tempenemy = "";
            var enemy = TargetSelector.GetSelectedTarget();
            foreach (var buff in Player.Buffs)
                temp += buff.DisplayName + "(" + buff.Count + ")" + ", ";
            if ((enemy != null) && enemy.IsValid && enemy.IsValidTarget())
                foreach (var buff in enemy.Buffs)
                    tempenemy += buff.DisplayName + "(" + buff.Count + ")" + ", ";
            if ((enemy != null) && enemy.IsValid && enemy.IsValidTarget())
                foreach (var buff in enemy.Buffs)
                    tempenemy += buff.DisplayName + "(" + buff.Count + ")" + ", ";

            var spellQ = Player.Spellbook.GetSpell(SpellSlot.Q);
            var dataQ = Player.Spellbook.GetSpell(SpellSlot.Q).SData;
            var spellW = Player.Spellbook.GetSpell(SpellSlot.W);
            var dataW = Player.Spellbook.GetSpell(SpellSlot.W).SData;
            var spellE = Player.Spellbook.GetSpell(SpellSlot.E);
            var dataE = Player.Spellbook.GetSpell(SpellSlot.E).SData;
            var spellR = Player.Spellbook.GetSpell(SpellSlot.R);
            var dataR = Player.Spellbook.GetSpell(SpellSlot.R).SData;

            if (Config.Item("ActiveConsole").GetValue<KeyBind>().Active)
            {
                Console.WriteLine("Coordinates:" + Player.Position);
                Console.WriteLine("Gold Earned: " + Player.GoldTotal);
                Console.WriteLine("Attack Delay: " + Player.AttackDelay);
                Console.WriteLine("Chance of Critical: " + PlayerCrit);
                Console.WriteLine("Wards Destroyed: ", Player.WardsKilled);
                Console.WriteLine("Wards Placed: ", Player.WardsPlaced);
                Console.WriteLine("Wards Bought: ", Player.SightWardsBought + Player.VisionWardsBought.ToString());
                Console.WriteLine("Last SpellCasted" + Player.LastCastedSpellName());
                Console.WriteLine("Player Direction:" + Player.Direction);
                Console.WriteLine("Base AD: " + Player.BaseAttackDamage);
                Console.WriteLine("Base AP: " + Player.BaseAbilityDamage);
                Console.WriteLine("Experience: " + Player.Experience);
                Console.WriteLine("Cursor Position: " + Game.CursorPos);
                Console.WriteLine("Player Buffs: " + temp);
                Console.WriteLine("Enemy Buffs: " + tempenemy);
                Console.WriteLine("Q Name:" + spellQ.Name);
                Console.WriteLine("Q Level:" + spellQ.Level);
                Console.WriteLine("Q Range:" + spellQ.SData.CastRange);
                Console.WriteLine("W Name:" + spellW.Name);
                Console.WriteLine("W Level:" + spellW.Level);
                Console.WriteLine("W Range:" + spellW.SData.CastRange);
                Console.WriteLine("E Name:" + spellE.Name);
                Console.WriteLine("E Level:" + spellE.Level);
                Console.WriteLine("E Range:" + spellE.SData.CastRange);
                Console.WriteLine("R Name:" + spellR.Name);
                Console.WriteLine("R Level:" + spellR.Level);
                Console.WriteLine("R Range:" + spellR.SData.CastRange);
            }
            Drawing.DrawText(10, 0, System.Drawing.Color.Red, "DevPrivate by eXtraGoZ");
            Drawing.DrawText(10, 10, System.Drawing.Color.White, "Coordinates:");
            Drawing.DrawText(10, 25, System.Drawing.Color.White, Player.Position.ToString());
            Drawing.DrawText(10, 55, System.Drawing.Color.White, "General Info:");
            Drawing.DrawText(10, 70, System.Drawing.Color.White, "Gold Earned: " + Player.GoldTotal);
            Drawing.DrawText(10, 85, System.Drawing.Color.White, "Attack Delay: " + Player.AttackDelay);
            Drawing.DrawText(10, 100, System.Drawing.Color.White, "Chance of Critical: " + PlayerCrit);
            Drawing.DrawText(10, 130, System.Drawing.Color.White, "Wards:");
            Drawing.DrawText(10, 145, System.Drawing.Color.White, "Wards Destroyed: ", Player.WardsKilled.ToString());
            Drawing.DrawText(10, 160, System.Drawing.Color.White, "Wards Placed: ", Player.WardsPlaced.ToString());
            Drawing.DrawText(10, 175, System.Drawing.Color.White, "Wards Bought: ",
                Player.SightWardsBought + Player.VisionWardsBought.ToString());
            Drawing.DrawText(10, 195, System.Drawing.Color.White, "Last Spell Casted:");
            Drawing.DrawText(10, 210, System.Drawing.Color.White, Player.LastCastedSpellName());
            Drawing.DrawText(10, 225, System.Drawing.Color.White, "Player Direction:");
            Drawing.DrawText(10, 240, System.Drawing.Color.White, Player.Direction.ToString());
            Drawing.DrawText(10, 265, System.Drawing.Color.White, "Base AD: " + Player.BaseAttackDamage);
            Drawing.DrawText(10, 280, System.Drawing.Color.White, "Base AP: " + Player.BaseAbilityDamage);
            Drawing.DrawText(10, 305, System.Drawing.Color.White, "Experience: " + Player.Experience);
            Drawing.DrawText(10, 325, System.Drawing.Color.White, "Cursor Position: " + Game.CursorPos);
            Drawing.DrawText(10, 355, System.Drawing.Color.White, "Player Buffs: ");
            Drawing.DrawText(10, 370, System.Drawing.Color.White, temp);
            Drawing.DrawText(10, 385, System.Drawing.Color.White, "Enemy Buffs: ");
            Drawing.DrawText(10, 400, System.Drawing.Color.White, tempenemy);

            Drawing.DrawText(400, 0, System.Drawing.Color.White, "Skill Info:");
            Drawing.DrawText(400, 25, System.Drawing.Color.White, "Q: ");
            Drawing.DrawText(400, 40, System.Drawing.Color.White, "--------");
            Drawing.DrawText(400, 50, System.Drawing.Color.White, "Name: " + spellQ.Name);
            Drawing.DrawText(400, 65, System.Drawing.Color.White, "Level: " + spellQ.Level);
            Drawing.DrawText(400, 80, System.Drawing.Color.White, "Range: " + spellQ.SData.CastRange);
            Drawing.DrawText(400, 100, System.Drawing.Color.White, "W: ");
            Drawing.DrawText(400, 115, System.Drawing.Color.White, "--------");
            Drawing.DrawText(400, 130, System.Drawing.Color.White, "Name: " + spellW.Name);
            Drawing.DrawText(400, 145, System.Drawing.Color.White, "Level: " + spellW.Level);
            Drawing.DrawText(400, 160, System.Drawing.Color.White, "Range: " + spellW.SData.CastRange);
            Drawing.DrawText(400, 180, System.Drawing.Color.White, "E: ");
            Drawing.DrawText(400, 195, System.Drawing.Color.White, "--------");
            Drawing.DrawText(400, 210, System.Drawing.Color.White, "Name: " + spellE.Name);
            Drawing.DrawText(400, 225, System.Drawing.Color.White, "Level: " + spellE.Level);
            Drawing.DrawText(400, 240, System.Drawing.Color.White, "Range: " + spellE.SData.CastRange);
            Drawing.DrawText(400, 280, System.Drawing.Color.White, "R: ");
            Drawing.DrawText(400, 295, System.Drawing.Color.White, "--------");
            Drawing.DrawText(400, 310, System.Drawing.Color.White, "Name: " + spellR.Name);
            Drawing.DrawText(400, 325, System.Drawing.Color.White, "Level: " + spellR.Level);
            Drawing.DrawText(400, 340, System.Drawing.Color.White, "Range: " + spellR.SData.CastRange);
            Drawing.DrawText(0, 10, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 20, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 30, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 40, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 50, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 60, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 70, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 80, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 90, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 100, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 110, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 120, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 130, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 140, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 150, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 160, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 170, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 180, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 190, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 200, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 210, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 220, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 230, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 240, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 250, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 260, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 270, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 280, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 290, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 300, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 310, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 320, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 330, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 340, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 350, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 360, System.Drawing.Color.Red, "|");
            Drawing.DrawText(0, 370, System.Drawing.Color.Red, "|");

            //
            Drawing.DrawText(390, 0, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 10, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 20, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 30, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 40, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 50, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 60, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 70, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 80, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 90, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 100, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 110, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 120, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 130, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 140, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 150, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 160, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 170, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 180, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 190, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 200, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 210, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 220, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 230, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 240, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 250, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 260, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 270, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 280, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 290, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 300, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 310, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 320, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 330, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 335, System.Drawing.Color.Red, "|");
            Drawing.DrawText(390, 340, System.Drawing.Color.Red, "|");
        }

        private static void OnDraw(EventArgs args)
        {
        }
    }
}