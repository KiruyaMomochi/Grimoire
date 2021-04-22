using System;
using System.Collections.Generic;
using Grimoire.Parser.Parameters;

namespace Grimoire.Parser
{
    public class Parser
    {
        public delegate Parameter CommandParser(string arg);

        private readonly Dictionary<string, CommandParser> _groupParsers = new();
        private readonly Dictionary<string, CommandParser> _userParsers = new();

        public Parser AddGroupParser(CommandParser commandParser, string command)
        {
            _groupParsers.Add(command, commandParser);
            return this;
        }

        public Parser AddGroupParser(CommandParser commandParser, IEnumerable<string> commands)
        {
            foreach (var command in commands) AddGroupParser(commandParser, command);
            return this;
        }
        
        public Parser AddGroupParser(CommandParser commandParser, params string[] commands)
        {
            foreach (var command in commands) AddGroupParser(commandParser, command);
            return this;
        }
        
        public Parser AddUserParser(CommandParser commandParser, string command)
        {
            _userParsers.Add(command, commandParser);
            return this;
        }

        public Parser AddUserParser(CommandParser commandParser, IEnumerable<string> commands)
        {
            foreach (var command in commands) AddUserParser(commandParser, command);
            return this;
        }
        
        public Parser AddUserParser(CommandParser commandParser, params string[] commands)
        {
            foreach (var command in commands) AddUserParser(commandParser, command);
            return this;
        }

        private static string SplitToken(in string message, out string remain)
        {
            var tokens = message.Split(
                (char[])null,
                2,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            if (tokens.Length == 0)
                return remain = null;

            remain = tokens.Length == 1 ? string.Empty : tokens[1];
            return tokens[0];
        }
        
        public Parameter ParseGroup(in string message)
        {
            var command = SplitToken(message, out var remain);
            if (_groupParsers.TryGetValue(command, out var parser))
                return parser(remain);
            return NoneParameter.Empty;
        }
        
        public Parameter ParseUser(in string message)
        {
            var command = SplitToken(message, out var remain);
            if (_userParsers.TryGetValue(command, out var parser))
                return parser(remain);
            return NoneParameter.Empty;
        }
    }
}