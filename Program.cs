﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuffPanel
{
    class Program
    {
        static void Main(string[] args)
        {
            BuffPanel.Track("kokosynasnehu", DateTime.Now.ToString());
            Console.ReadKey();
        }
    }
}
