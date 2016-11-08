using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TutorialForSoviet
{
    class Program
    {
        static void Main(string[] args)
        {
            int o = 10;
            for (int i = 0; i < o; i++)
            {
                Console.WriteLine(i);
            }
            Console.ReadKey();
        }
    }
}
