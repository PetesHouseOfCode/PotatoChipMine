using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Globalization;
using System.Linq;

namespace PotatoChipMine.Core.Commands
{
    public static class InspectCommandTypes
    {
        public const string Default = "Default";
        public const string LifeTime = "LifeTime";
        public const string Upgrades = "Upgrades";
    }

    public class InspectCommand : CommandWithGameState, ICommand
    {
        public string DiggerName { get; set; }
        public string InspectType { get; set; }
    }

    public class InspectCommandHandler : ICommandHandler<InspectCommand>
    {
        public void Handle(InspectCommand command)
        {
            var gameState = command.GameState;

            if (!gameState.Miner.Diggers.Any(x =>
                string.Equals(x.Name, command.DiggerName, StringComparison.CurrentCultureIgnoreCase)))
            {
                Game.WriteLine($"Could not find digger named {command.DiggerName}", PcmColor.Red);
                return;
            }

            var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                string.Equals(x.Name, command.DiggerName, StringComparison.CurrentCultureIgnoreCase));

            switch (command.InspectType)
            {
                case InspectCommandTypes.Default:
                    ReportDiggerIdentityInfo(digger);
                    ReportDiggerCoreStats(digger);
                    break;
                case InspectCommandTypes.LifeTime:
                    ReportDiggerIdentityInfo(digger);
                    ReportDiggerLifetimeStats(digger);
                    break;
                case InspectCommandTypes.Upgrades:
                    ReportDiggerUpgrades(digger);
                    break;
            }

            return;
        }

        private static void ReportDiggerIdentityInfo(ChipDigger digger)
        {
            Game.ClearConsole();
            var headTable = new TableOutput(80, PcmColor.Yellow);
            headTable.AddHeaders("Name", "Class", "Equipped Date");
            headTable.AddRow($"{digger.Name}", $"{digger.Class.ToString()}",
                $"{digger.FirstEquipped}");
            Game.Write(headTable);
        }

        private static void ReportDiggerCoreStats(ChipDigger digger)
        {
            var vitalsTable = new TableOutput(80, PcmColor.Yellow);
            vitalsTable.AddHeaders("Stat", "Value");
            vitalsTable.AddRow("Site Hardness", digger.MineClaim.Hardness.ToString());
            vitalsTable.AddRow("Site Chip Density", digger.MineClaim.ChipDensity.ToString());
            vitalsTable.AddRow("Durablity (Left) / (Max)", $"{digger.Durability.Current} / {digger.Durability.Max}");
            vitalsTable.AddRow("Hopper", digger.Hopper.Name);
            vitalsTable.AddRow("Hopper Space (Left) / (Max)",
                $"{digger.Hopper.Max - digger.Hopper.Count} / {digger.Hopper.Max}");
            Game.Write(vitalsTable);
        }

        private static void ReportDiggerLifetimeStats(ChipDigger digger)
        {
            Game.WriteLine("Lifetime Statistics", PcmColor.Black, PcmColor.Yellow);
            var lifetimeTable = new TableOutput(80, PcmColor.Yellow);
            lifetimeTable.AddHeaders("Stat", "Value");
            lifetimeTable.AddRow("First Equipped", digger.FirstEquipped.ToString(CultureInfo.CurrentCulture));
            lifetimeTable.AddRow("Lifetime Digs", digger.GetLifeTimeStat(DiggerStats.LifetimeDigs).ToString());
            lifetimeTable.AddRow("Lifetime Chips", digger.GetLifeTimeStat(DiggerStats.LifetimeChips).ToString());
            lifetimeTable.AddRow("Lifetime Repairs", digger.GetLifeTimeStat(DiggerStats.LifetimeRepairs).ToString());
            lifetimeTable.AddRow("Lifetime Bolts Cost", digger.GetLifeTimeStat(DiggerStats.LifeTimeBoltsCost).ToString());
            lifetimeTable.AddRow("Lifetime Tokes Cost", digger.GetLifeTimeStat(DiggerStats.LifeTimeTokensCost).ToString());
            Game.Write(lifetimeTable);
        }

        private static void ReportDiggerUpgrades(ChipDigger digger)
        {
            Game.WriteLine("Available Upgrades", PcmColor.Black, PcmColor.Yellow);
            var upgradesTable = new TableOutput(80, PcmColor.Yellow);
            upgradesTable.AddHeaders("Name", "Max Level", "Current Level", "Slot");
            foreach (var diggerUpgrade in digger.AvailableUpgrades)
            {
                upgradesTable.AddRow(diggerUpgrade.Name,
                    diggerUpgrade.MaxLevel.ToString(),
                    diggerUpgrade.CurrentLevel.ToString(),
                    diggerUpgrade.Slot.ToString()
                );
                upgradesTable.AddRow(diggerUpgrade.Description);
            }

            Game.Write(upgradesTable);
        }
    }
}
