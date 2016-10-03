using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data;
using SharpDX;

namespace SurvivorSeriesAIO.Utility
{
    class SkinChanger
    {
        public Menu Config = Program.Config;
        public Obj_AI_Hero Player { get { return ObjectManager.Player; } }

        public void LoadSkinsSS()
        {
            /*switch (Player.ChampionName)
            {
                case "Ashe":
                    break;
            }
            Config.SubMenu("Skins Menu").AddItem(new MenuItem("SkinID", "Skin ID")).SetValue(new StringList(new[] { "Ashe1", "Ashe2", "Ashe3", "Ashe4", "Ashe5", "Ashe6", "Ashe7" }, 6));
            */
        }
    }
}
