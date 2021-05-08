// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System.Collections.Generic;

namespace SourceAFIS.Cli.Utils.Args
{
    class CommandGroup
    {
        public readonly Dictionary<string, CommandGroup> Subcommands = new Dictionary<string, CommandGroup>();
        public readonly Dictionary<int, Command> Overloads = new Dictionary<int, Command>();

        public void Add(int depth, Command command)
        {
            if (depth < command.Subcommand.Length)
            {
                var key = command.Subcommand[depth];
                if (!Subcommands.ContainsKey(key))
                    Subcommands[key] = new CommandGroup();
                Subcommands[key].Add(depth + 1, command);
            }
            else
                Overloads[command.Parameters.Length] = command;
        }
    }
}
