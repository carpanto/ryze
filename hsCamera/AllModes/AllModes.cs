using hsCamera.Handlers;
using LeagueSharp.Common;

namespace hsCamera.AllModes
{
    class AllModes : Program
    {
        public static void CameraMode()
        {
            if (_config.Item("LastHit").GetValue<KeyBind>().Active)
            {
                Modes.FarmTracker();
            }
            if (_config.Item("LaneClear").GetValue<KeyBind>().Active)
            {
                Modes.FarmTracker();
            }
            if (_config.Item("Orbwalk").GetValue<KeyBind>().Active)
            {
                Modes.EnemyTracker();
            }
        }
    }
}

