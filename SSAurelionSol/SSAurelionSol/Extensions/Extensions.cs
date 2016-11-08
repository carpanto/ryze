using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace SSAurelionSol.Extensions
{
    class Extensions
    {
        private static Obj_AI_Hero Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        public static bool IsWToggled()
        {
            if (Player.HasBuff("AurelionSolWActive")) return true;
            return false;
        }
    }
}
