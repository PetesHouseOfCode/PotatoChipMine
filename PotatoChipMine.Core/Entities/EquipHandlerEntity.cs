using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System;

namespace PotatoChipMine.Core.Entities
{
    public class EquipHandlerEntity : GameEntity
    {
        private string diggerName;

        private bool hasSetName;
        private bool hasSelectedClaim;

        readonly bool claimsAvailable = false;

        public EquipHandlerEntity(GameState gameState)
            : base(gameState)
        {
        }

        public EquipHandlerEntity(GameState gameState, string diggerName)
            : base(gameState)
        {
            this.hasSetName = true;
            this.diggerName = diggerName;
        }

        public override void HandleInput(UserCommand command)
        {
            if (!hasSetName)
            {
                if (string.IsNullOrEmpty(command.FullCommand))
                {
                    Game.WriteLine("A name is required!", PcmColor.Red);
                    return;
                }

                diggerName = command.FullCommand.Trim().Replace(" ", "-");

                if (GameState.Miner.Diggers.Exists(x => x.Name == diggerName))
                {
                    Game.WriteLine($"Digger with the name {diggerName} already exists.", PcmColor.Red);
                    return;
                }
            }

            if (claimsAvailable)
            {
                if (!hasSelectedClaim)
                {
                    ShowClaims();
                    GameState.PromptText = "Enter Claim Id: ";
                    return;
                }

                /*
                TestSelectedClaim();
                if (!validClaimSelected)
                {
                    ShowClaims();
                    GameState.PromptText = "Enter Claim Id: ";
                    return;
                }

                EquipDigger(mineClaimFromLease);
                */
            }
            else
            {
                EquipDigger(new MineSiteFactory().BuildSite());
            }

            GameState.PromptText = null;
            Game.PopScene();
        }

        public override void Update(Frame frame)
        {
            if (hasSetName)
            {
                EquipDigger(new MineSiteFactory().BuildSite());
                GameState.PromptText = null;
                Game.PopScene();
            }
        }

        private void EquipDigger(MineClaim mineClaim)
        {
            var digger = GameState.Miner.Inventory("standard_digger");
            var newDigger = ChipDigger.StandardDigger(mineClaim);
            newDigger.Name = diggerName;
            digger.Count--;
            GameState.Miner.Diggers.Add(newDigger);

            Game.Write($"Digger {newDigger.Name} has been equipped on ");
            Game.Write($"{newDigger.MineSite.ChipDensity.ToString()} density", PcmColor.Blue);
            Game.Write(" with a ");
            Game.Write($"{newDigger.MineSite.Hardness.ToString()} hardness", PcmColor.Cyan);
            Game.WriteLine(string.Empty);

        }
        void ShowClaims()
        {
            Game.WriteLine("1. Claim Details.... coming soon.");
        }
    }
}