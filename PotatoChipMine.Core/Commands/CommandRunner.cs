using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PotatoChipMine.Core.Commands
{
    public static class CommandRunner
    {
        private static Dictionary<Type, Type> handlers = new Dictionary<Type, Type>();

        public static void Run(ICommand command)
        {
            if (!handlers.Any())
                LoadCommandHandlers();

            var handler = handlers[command.GetType()];
            var c = Activator.CreateInstance(handler);
            var method = handler.GetMethod("Handle");
            method.Invoke(c, new object[] { command });
        }

        private static void LoadCommandHandlers()
        {
            var openGenericType = typeof(ICommandHandler<>);

            var commandHandlers = from x in Assembly.GetExecutingAssembly().GetTypes()
                                  from z in x.GetInterfaces()
                                  let y = x.BaseType
                                  where
                                  (y != null && y.IsGenericType &&
                                  openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                                  (z.IsGenericType &&
                                  openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition()))
                                  select x;

            foreach (var handler in commandHandlers)
            {
                handlers.Add(handler.GetTypeInfo().ImplementedInterfaces.First().GenericTypeArguments[0], handler);
            }
        }
    }
}
