using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PotatoChipMine.Core.Commands
{
    public class ViewClaimListingsCommand : ICommand
    {
        public ClaimListings Listings { get; set; }
    }

    public class ViewClaimListingsCommandHandler : ICommandHandler<ViewClaimListingsCommand>
    {
        public void Handle(ViewClaimListingsCommand command)
        {
            var table = new TableOutput(80, PcmColor.Green);
            table.AddHeaders("Id", "Price", "Density", "Hardness", "Rent");

            if (!command.Listings.GetAll().Any())
            {
                table.AddRow("No Claims Available");
                Game.Write(table);
                return;
            }

            foreach (var listing in command.Listings.GetAll())
            {
                table.AddRow(
                    listing.Id.ToString(),
                    listing.Price.ToString(), 
                    listing.Survey.Density,
                    listing.Survey.Hardness,
                    listing.LeasePrice.ToString());
            }

            Game.Write(table);
        }
    }
}
