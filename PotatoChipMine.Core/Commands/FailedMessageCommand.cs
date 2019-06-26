using PotatoChipMine.Core.GameEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoChipMine.Core.Commands
{
    public class FailedMessageCommand : ICommand
    {
        public string Message { get; }

        public FailedMessageCommand(string message)
        {
            Message = message;
        }
    }

    public class FailedMessageCommandHandler : ICommandHandler<FailedMessageCommand>
    {
        public void Handle(FailedMessageCommand command)
        {
            Game.WriteLine(command.Message, PcmColor.Red);
        }
    }
}
