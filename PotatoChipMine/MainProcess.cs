using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using PotatoChipMine.Models;

namespace PotatoChipMine
{
    public class MainProcess
    {
        private bool _running = true;
        private ChipDigger _digger;
        private int _chips = 0;

        private MineSite _mineSite = new MineSite()
        {
            ChipDensity = ChipDensity.Rich
        };

        public MainProcess()
        {
            _mineSite = new MineSite {ChipDensity = ChipDensity.Normal,Hardness = SiteHardness.Soft};
            _digger = new ChipDigger(_mineSite) {Durability = 25};
        }

        public void Run()
        {
            while (_running)
            {
                Console.WriteLine("Digging chips.");
                Thread.Sleep(3000);
                var scoop = _digger.Dig();
                _chips += scoop.Chips;
                Console.WriteLine($"{scoop.Chips} dug!");
                Console.WriteLine($"Digger durability: {_digger.Durability}");
                if (_digger.Durability == 0)
                {
                    _running = false;
                }
            }
            Console.WriteLine("Dig Complete");
            Console.WriteLine($"Chip yield:{_chips}");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }

}
