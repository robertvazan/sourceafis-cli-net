// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceAFIS.Cli.Utils.Args
{
    class CommandParser
    {
        private readonly CommandGroup CommandRoot = new CommandGroup();
        private readonly List<Command> Commands = new List<Command>();
        private readonly Dictionary<string, Option> OptionMap = new Dictionary<string, Option>();
        private readonly List<Option> Options = new List<Option>();
        public CommandParser Add(Command command)
        {
            CommandRoot.Add(0, command);
            Commands.Add(command);
            return this;
        }
        public CommandParser Add(Option option) {
            OptionMap[option.Name] = option;
            Options.Add(option);
            return this;
        }
        public Action Parse(string[] args)
        {
            if (args.Length == 0)
            {
                Pretty.Print("SourceAFIS CLI for .NET");
                Pretty.Print("");
                Pretty.Print("Available subcommands:");
                foreach (var registered in Commands)
                {
                    Pretty.Print(string.Format("\t{0}{1}",
                        string.Join(" ", registered.Subcommand),
                        string.Join("", registered.Parameters.Select(p => $" <{p}>").ToArray())));
                    Pretty.Print($"\t\t{registered.Description}");
                }
                Pretty.Print("");
                Pretty.Print("Available options:");
                foreach (var registered in Options)
                {
                    Pretty.Print(string.Format("\t--{0}{1}", registered.Name, string.Join("", registered.Parameters.Select(p => $" <{p}>").ToArray())));
                    Pretty.Print($"\t\t{registered.Description}");
                    if (registered.Fallback != null)
                        Pretty.Print($"\t\tDefault: {registered.Fallback}");
                }
                Environment.Exit(0);
            }
            int consumed = 0;
            var group = CommandRoot;
            var commandArgs = new List<string>();
            while (consumed < args.Length)
            {
                var arg = args[consumed];
                ++consumed;
                if (arg.StartsWith("--"))
                {
                    var name = arg.Substring(2);
                    if (!OptionMap.ContainsKey(name))
                        throw new ArgumentException($"Unknown option: {arg}");
                    var option = OptionMap[name];
                    var optionArgs = new List<string>();
                    for (int i = 0; i < option.Parameters.Length; ++i) {
                        if (consumed >= args.Length)
                            throw new ArgumentException($"Missing argument <{option.Parameters[i]}> for option '{arg}'.");
                        optionArgs.Add(args[consumed]);
                        ++consumed;
                    }
                    option.Run(optionArgs.ToArray());
                }
                else
                {
                    if (commandArgs.Count == 0 && group.Subcommands.ContainsKey(arg))
                        group = group.Subcommands[arg];
                    else
                        commandArgs.Add(arg);
                }
            }
            if (group == CommandRoot && commandArgs.Count == 0)
                throw new ArgumentException("Specify subcommand.");
            Command command;
            if (!group.Overloads.TryGetValue(commandArgs.Count, out command))
                throw new ArgumentException("Unrecognized subcommand.");
            return () => command.Run(commandArgs.ToArray());
        }
    }
}
