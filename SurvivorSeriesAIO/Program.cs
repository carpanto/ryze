// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="SurvivorSeriesAIO">
//      Copyright (c) SurvivorSeriesAIO. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SurvivorSeriesAIO.Core;
using SurvivorSeriesAIO.SurvivorMain;

namespace SurvivorSeriesAIO
{
    internal class Program
    {
        /// <summary>
        ///     SurvivorSeries AIO NEWS
        /// </summary>
        public static string SSNews = "Reminders/Improvements/JungleClear for Ryze Added";

        public static IRootMenu RootMenu { get; set; }

        public static IChampion Champion { get; set; }

        public static IActivator Activator { get; set; }

        private static Obj_AI_Hero Player => ObjectManager.Player;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += GameOnOnGameLoad;
        }

        /// <summary>
        ///     GameOnOnGameLoad - Load Every Plugin/Addon
        /// </summary>
        /// <param name="args"></param>
        private static void GameOnOnGameLoad(EventArgs args)
        {
            #region Subscriptions

            Game.PrintChat(
                "<font color='#0993F9'>[SurvivorSeries AIO]</font> <font color='#FF8800'>Successfully Loaded.</font>");

            Game.PrintChat("<font color='#b756c5'>[SurvivorSeries] NEWS: </font>" + SSNews);

            #endregion

            RootMenu = new RootMenu("SurvivorSeries AIO");

            #region Utility Loads

            new VersionCheck.VersionCheck().UpdateCheck();
            SpellCast.RootConfig = RootMenu;

            ChampionFactory.Load(ObjectManager.Player.ChampionName, RootMenu);

            ActivatorFactory.Create(ObjectManager.Player.ChampionName, RootMenu);

            AutoLevelerFactory.Create(ObjectManager.Player.ChampionName, RootMenu);

            #endregion
        }
    }
}