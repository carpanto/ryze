// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraMovement.cs.cs" company="hsCamera">
//      Copyright (c) hsCamera. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace hsCamera.Handlers
{
    internal class CameraMovement : Program
    {
        public static void SemiDynamic(Vector3 position)
        {
            var distance = Camera.Position.Distance(position);


            if (distance <= 1)
                return;

            var speed = Math.Max(0.2f, Math.Min(20, distance*0.0007f*20));
            switch (_config.Item("dynamicmode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                {
                    var direction = (position - Camera.Position).Normalized()*speed;
                    Camera.Position = direction + Camera.Position;
                }
                    break;
                case 1:
                {
                    var direction = (position - Camera.Position).Normalized()*
                                    _config.Item("followcurspeed").GetValue<Slider>().Value;
                    Camera.Position = direction + Camera.Position;
                }
                    break;
                case 2:
                {
                    var direction = (position - Camera.Position).Normalized()*
                                    _config.Item("followtfspeed").GetValue<Slider>().Value;
                    Camera.Position = direction + Camera.Position;
                }
                    break;
            }
        }
    }
}