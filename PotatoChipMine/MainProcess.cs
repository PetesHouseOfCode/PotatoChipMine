using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine
{
    public class MainProcess
    {
        private bool _running = true;
        private ChipDigger _digger;
        private int _chips = 0;
        private int _chipVault = 0;

        private MineSite _mineSite = new MineSite()
        {
            ChipDensity = ChipDensity.Rich
        };

        public MainProcess()
        {
            var siteFactory = new MineSiteFactory();
            _mineSite = siteFactory.BuildSite();
            _digger = new ChipDigger(_mineSite) {Durability = 25};
        }

        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Mine site density:{_mineSite.ChipDensity}.");
            Console.WriteLine($"Site Hardness:{_mineSite.Hardness}"); 
            Console.WriteLine("Goood luuuuck pilgrim.");
            Console.ResetColor();
            while (_running)
            {
                var oldDurabiliity = _digger.Durability;
                Console.WriteLine("Digging chips.");
                Thread.Sleep(3000);
                var scoop = _digger.Dig();
                var diggerDamage = oldDurabiliity - _digger.Durability;
                _chips += scoop.Chips;
                
                Console.WriteLine($"{scoop.Chips} dug!");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Digger Wear:{diggerDamage}");
                if (diggerDamage > 5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("OUCH!");
                }
                Console.ResetColor();
                Console.WriteLine($"Digger durability: {_digger.Durability}");
                if (_chips > 30)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Your chip hopper is full. Press [Enter] to empty chip hopper");
                    Console.ReadLine();
                    _chipVault += _chips;
                    _chips = 0;
                    Console.ResetColor();
                }
                if (_digger.Durability == 0)
                {
                    _running = false;
                }
            }
            Console.WriteLine("Dig Complete");
            Console.WriteLine($"Chip yield:{_chips}");
            Console.WriteLine($"Chip vault:{_chipVault}");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }

}
