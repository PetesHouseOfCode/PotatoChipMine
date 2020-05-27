using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Models.Claims;
using PotatoChipMine.Core.Services;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace PotatoChipMine.Core.Entities
{
    public enum EquipHandlerState
    {
        Starting,
        AskingDiggerName,
        AskingClaimLeaseId
    }

    public class EquipHandlerEntity : GameEntity
    {
        private int claimLeaseId;
        private string diggerName;
               
        readonly bool claimsAvailable = true;
        private EquipHandlerState state = EquipHandlerState.Starting;

        public EquipHandlerEntity(GameState gameState)
            : base(gameState)
        {
        }

        public EquipHandlerEntity(GameState gameState, string diggerName, int claimLeaseId)
            : base(gameState)
        {
            this.diggerName = diggerName;
            this.claimLeaseId = claimLeaseId;
        }

        public override void HandleInput(UserCommand command)
        {
            if (state == EquipHandlerState.AskingDiggerName)
            {
                if (string.IsNullOrEmpty(command.FullCommand))
                {
                    Game.WriteLine("A name is required!", PcmColor.Red);
                    return;
                }

                if (DiggerWithNameExists(FormatDiggerName(command.CommandText)))
                {
                    Game.WriteLine($"Digger with the name {diggerName} already exists.", PcmColor.Red);
                    return;
                }

                diggerName = FormatDiggerName(command.CommandText);
            }

            if (claimsAvailable)
            {

                if(state != EquipHandlerState.AskingClaimLeaseId)
                {
                    state = EquipHandlerState.AskingClaimLeaseId;
                    ShowClaims();
                    GameState.PromptText = "Enter Claim Id: ";
                    return;
                }

                if(!int.TryParse(command.CommandText, out int id))
                {
                    Game.WriteLine($"\"{command.CommandText}\" is not a number.");
                    return;
                }

                if(ClaimWithIdDoesNotExists(id))
                {
                    Game.WriteLine($"Claim Lease with {command.CommandText} id doesn't exist.");
                    return;
                }

                var claimLease = GameState.Miner.ClaimLeases.GetAll().First(x => x.Id == id);

                if(claimLease.InUse)
                {
                    Game.WriteLine($"Claim Lease with {id} id is already in use.");
                    return;
                }

                claimLease.AssignDigger(EquipDigger(claimLease.Claim));

                GameState.PromptText = null;
                Game.PopScene();
                return;
            }

            EquipDigger(new MineSiteFactory().BuildSite());

            GameState.PromptText = null;
            Game.PopScene();
        }

        private bool ClaimWithIdDoesNotExists(int claimLeaseId)
        {
            return GameState.Miner.ClaimLeases.HasId(claimLeaseId) == false;
        }

        public override void Update(Frame frame)
        {
            if (state == EquipHandlerState.Starting)
            {
                if(MinerCanNotEquip())
                {
                    Game.WriteLine("You can't do this at this time.");
                    GameState.PromptText = null;
                    Game.PopScene();
                    return;
                }

                if(string.IsNullOrEmpty(diggerName))
                {
                    Game.WriteLine("A name is required!", PcmColor.Red);
                    state = EquipHandlerState.AskingDiggerName;
                    GameState.PromptText = "Enter Digger Name: ";
                    return;
                }

                if(DiggerWithNameExists(FormatDiggerName(diggerName)))
                {
                    Game.WriteLine($"Digger with the name {diggerName} already exists.", PcmColor.Red);
                    state = EquipHandlerState.AskingDiggerName;
                    GameState.PromptText = "Enter Digger Name: ";
                    return;
                }

                if (claimsAvailable)
                {
                    if (claimLeaseId <= 0)
                    {
                        ShowClaims();
                        state = EquipHandlerState.AskingClaimLeaseId;
                        GameState.PromptText = "Enter Claim Id: ";
                        return;
                    }

                    var claimLease = GameState.Miner.ClaimLeases.GetAll().First(x => x.Id == claimLeaseId);

                    if (claimLease.InUse)
                    {
                        ShowClaims();
                        Game.WriteLine($"Claim Lease with {claimLeaseId} id is already in use.");
                        state = EquipHandlerState.AskingClaimLeaseId;
                        GameState.PromptText = "Enter Claim Id: ";
                        return;
                    }

                    diggerName = FormatDiggerName(diggerName);
                    claimLease.AssignDigger(EquipDigger(claimLease.Claim));
                    GameState.PromptText = null;
                    Game.PopScene();
                    return;
                }

                diggerName = FormatDiggerName(diggerName);
                EquipDigger(new MineSiteFactory().BuildSite());
                GameState.PromptText = null;
                Game.PopScene();
            }
        }

        private bool MinerCanNotEquip()
        {
            return MinerDoesNotHaveDiggers() || MinerDoesNotHaveRequiredClaims();
        }

        private bool MinerDoesNotHaveRequiredClaims()
        {
            return claimsAvailable && !GameState.Miner.ClaimLeases.HasClaimsAvailable();
        }

        private bool MinerDoesNotHaveDiggers()
        {
            var digger = GameState.Miner.Inventory("standard_digger");
            return digger == null || digger.Count <= 0;
        }

        private bool DiggerWithNameExists(string diggerName)
        {
            return GameState.Miner.Diggers.Exists(x => x.Name == diggerName);
        }

        private string FormatDiggerName(string diggerNameRaw)
        {
            return diggerNameRaw.Trim().Replace(" ", "-");
        }

        private ChipDigger EquipDigger(MineClaim mineClaim)
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
            return newDigger;

        }
        void ShowClaims()
        {
            var table = new TableOutput(80);
            table.AddHeaders("Id", "Price", "Density", "Hardness");
            foreach (var claimLease in GameState.Miner.ClaimLeases.GetAll())
            {
                table.AddRow(
                    claimLease.Id.ToString(),
                    claimLease.Price.ToString(),
                    claimLease.Claim.ChipDensity.ToString(),
                    claimLease.Claim.Hardness.ToString());
            }

            Game.Write(table);
        }
    }
}