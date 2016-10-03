// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivatorBase.cs" company="SurvivorSeriesAIO">
//     Copyright (c) SurvivorSeriesAIO. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using LeagueSharp;

namespace SurvivorSeriesAIO.Core
{
    public abstract class AutoLevelerBase : IAutoLeveler
    {
        protected AutoLevelerBase(IRootMenu menu)
        {
            Menu = menu;
        }

        protected IRootMenu Menu { get; }

        protected Obj_AI_Hero Player { get; } = ObjectManager.Player;
    }
}