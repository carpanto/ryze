using System;
using Activator = SurvivorSeriesAIO.Utility.Activator;

namespace SurvivorSeriesAIO.Core
{
    public static class ActivatorFactory
    {
        public static IActivator Create(string name, IRootMenu menu)
        {
            switch (name)
            {
                case "Ryze":
                    return new Activator(menu);

                case "Malzahar":
                    return new Activator(menu);

                case "Ashe":
                    return new Activator(menu);

                case "Brand":
                    return new Activator(menu);

                case "Irelia":
                    return new Activator(menu);

                default:
                    //return new Utility.Activator(menu);
                    throw new NotSupportedException();
            }
        }
    }
}