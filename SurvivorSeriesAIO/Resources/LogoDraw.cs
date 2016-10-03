using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Drawing;

namespace SurvivorSeriesAIO.Resources
{
    class LogoDraw
    {
        private Menu Config = Program.Config;
        private Render.Sprite sslogo;
        /// <summary>
        /// LoadSS -> Render the SSLogo
        /// </summary>
        public void LoadSS()
        {
            Config.SubMenu("[SS] Settings").SetFontStyle(FontStyle.Bold, SharpDX.Color.GreenYellow).SubMenu("[SS] Misc").AddItem(new MenuItem("SSLogo", "[SS] Logo").SetValue(true));
            Config.SubMenu("[SS] Settings").SetFontStyle(FontStyle.Bold, SharpDX.Color.GreenYellow).SubMenu("[SS] Misc").AddItem(new MenuItem("CreditsHeader", "                             .:Credits:."));
            Config.SubMenu("[SS] Settings").SetFontStyle(FontStyle.Bold, SharpDX.Color.GreenYellow).SubMenu("[SS] Misc").AddItem(new MenuItem("CreditsHeaderInfo", "Developer: SupportExTraGoZ").SetFontStyle(FontStyle.Bold, SharpDX.Color.DeepPink));
            Config.SubMenu("[SS] Settings").SetFontStyle(FontStyle.Bold, SharpDX.Color.GreenYellow).SubMenu("[SS] Misc").AddItem(new MenuItem("CreditsHeaderInfo2", "Designer: gimleey").SetFontStyle(FontStyle.Bold, SharpDX.Color.DeepPink));
            Config.SubMenu("[SS] Settings").SetFontStyle(FontStyle.Bold, SharpDX.Color.GreenYellow).SubMenu("[SS] Misc").AddItem(new MenuItem("CreditsHeaderInfo3", "General Ideas: Media/Rethought").SetFontStyle(FontStyle.Bold, SharpDX.Color.DeepPink));
            if (Program.Config.Item("SSLogo").GetValue<bool>())
            {
                sslogo = new Render.Sprite(LoadImg("sslogo"), new Vector2((Drawing.Width / 2) - 500, (Drawing.Height / 2) - 350));
                sslogo.Add(0);
                sslogo.OnDraw();
            }

            LeagueSharp.Common.Utility.DelayAction.Add(8000, () => sslogo.Remove());
        }
        /// <summary>
        /// BitMap LoadImg
        /// </summary>
        /// <param name="imgName"></param>
        /// <returns></returns>
        private static Bitmap LoadImg(string imgName)
        {
            var bitmap = Resource1.ResourceManager.GetObject(imgName) as Bitmap;
            if (bitmap == null)
            {
                Console.WriteLine(imgName + "If you see that report in the thread: [Error] No Logo Found. However The Assembly will continue to work.");
            }
            return bitmap;
        }
    }
}
