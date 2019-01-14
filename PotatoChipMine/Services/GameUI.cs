using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using PotatoChipMine.Models;
using PotatoChipMine.Store.Models;

namespace PotatoChipMine.Services
{
    public class GameUI
    {
        
        public void ReportInfo(string[] linesToReport)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            TypeWriterWrite(linesToReport.ToList());
            Console.ResetColor();
        }

        public void FastWrite(string[] linesToReport,ConsoleColor color = ConsoleColor.Cyan)
        {
            Console.ForegroundColor = color;
            TypeWriterWrite(linesToReport.ToList(),3);
            Console.ResetColor();
        }

        public void WritePrompt(string linesToReport)
        {
            Console.ForegroundColor = ConsoleColor.White;
            TypeWriterWrite(new List<string>(){linesToReport},3);
            Console.ResetColor();
        }

        public UserCommand AcceptUserCommand(string commandContext ="")
        {
            var cmdStr = "Enter Command >>";
            if (commandContext != "")
                cmdStr = $"Enter {commandContext} Command >>";
            WritePrompt(cmdStr);
            var commandEntry = Console.ReadLine()?.Split(' ');
            if (commandEntry == null || commandEntry.Length == 0)
                return new EmptyCommand();
            return new UserCommand {CommandText = commandEntry?[0], Parameters = commandEntry.Skip(1).ToList()};
        }

        public void ReportDiggersStarting(List<ChipDigger> diggers)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            foreach (var chipDigger in diggers)
            {
                TypeWriterWrite(new List<string>
                {
                    $"{chipDigger.Name}",
                    $"---Mine site density:{ chipDigger.MineSite.ChipDensity}.",
                    $"---Site Hardness:{chipDigger.MineSite.Hardness}"
                });
                Console.WriteLine("");
            }
            Console.ResetColor();
        }

        public void ReportVault(GameState gameState)
        {
            FastWrite(new[] {$"Chip Vault: {gameState.Miner.Inventory("chips").Count}"});
            Console.WriteLine();
        }

        public void ReportAvailableCommands(CommandsGroup commandsGroup, GameState gameState)
        {
            FastWrite(new string[]
            {
                $"-----------   {gameState.Mode.ToString().ToUpper()} Commands  ---------------",
                "These commands are available from any game room."
            });
            foreach (var commandsDefinition in commandsGroup.LocalCommands)
            {
                var command = commandsDefinition.EntryDescription ?? commandsDefinition.Command;
                FastWrite(new[]
                    {$"Command: [{command}]", $"Description: {commandsDefinition.Description}", "--------"});
            }

            if (commandsGroup.ParentGroup == null || !(commandsGroup.ParentGroup.LocalCommands?.Count > 0)) return;

            FastWrite(new[]
                {"-----------   Global Commands  ---------------", "These commands are available from any game room."},
                ConsoleColor.Green);
            Console.WriteLine();
            foreach (var globalCommand in commandsGroup.ParentGroup.LocalCommands)
            {
                var command = globalCommand.EntryDescription ?? globalCommand.Command;
                FastWrite(new[]
                    {$"Command: [{command}]", $"Description: {globalCommand.Description}", "--------"},
                    ConsoleColor.Green);
            }


        }

        public void ReportStoreStock(List<StoreItem> stock, ConsoleColor color = ConsoleColor.Gray)
        {
            var inv = stock
                .Select(x => $"ID:{x.InventoryId} Item Name:{x.Name} Item Price:{x.Price} Quantity:{x.Count}")
                .ToArray();
            FastWrite(inv);
        }

        public void ReportBuyingItems(List<StoreItem> buying, ConsoleColor color = ConsoleColor.Gray)
        {
            var list = buying.Select(x => $"Item Name:{x.Name} Price Paid:{x.Price} tt").ToArray();
            FastWrite(list);
        }

        private void TypeWriterWrite(List<string>linesList,int charSpeed = 3)
        {
            foreach (var line in linesList)
            {
                foreach (var x in line)
                {
                    Console.Write(x);
                    Thread.Sleep(charSpeed);
                }

                Console.Write(Environment.NewLine);
                Thread.Sleep(100);
            }
        }

        public void ReportMinerState(Miner miner)
        {
            var minerState = new List<string>
            {
                $"Name: {miner.Name}",
                $"Chip Vault:{miner.Inventory("chips").Count}",
                $"Tater Tokens:{miner.TaterTokens}",
                $"Diggers Count:{miner.Diggers.Count}"
            };
            if(miner.Diggers.Count > 0)
                minerState.Add("----------Digger Detail-----------");
            foreach (var minerDigger in miner.Diggers)
            {
                minerState.Add($"Name:{minerDigger.Name} Durability:{minerDigger.Durability} Chips In Hopper:{minerDigger.Hopper}");
            }
            FastWrite(minerState.ToArray());
        }

        public void ReportMinerInventory(Miner miner)
        {
           var inv = miner.InventoryItems
            .Select(x => $"--> ID:{x.ItemId} Item Name:{x.Name} Quantity:{x.Count}")
                .ToArray();
            if (!inv.Any())
            {
                FastWrite(new[]{"Inventory is empty."});
                return;
            }
            FastWrite(inv);
        }

        public void ReportDiggerEquipped(string newDiggerName)
        {
            FastWrite(new [] {$"Digger {newDiggerName} is has been equipped"},ConsoleColor.Yellow);
        }

        public void ReportBadCommand(string badCommand)
        {
            FastWrite(new[] {$"{badCommand} is not a valid command.", "Type [help] to see a list of commands."},
                ConsoleColor.Red);
        }

        public void ReportException(string[] message)
        {
            FastWrite(message,ConsoleColor.Red);
        }

        public void ReportHopperEmptied(string diggerName, int hopperCount, int vaultCount)
        {
            FastWrite(
                new[]
                {
                    $"{hopperCount} was removed from {diggerName}'s hopper and moved into the chip vault.",
                    $"Vault Chips:{vaultCount}"
                }, ConsoleColor.Yellow);
        }
    }
}