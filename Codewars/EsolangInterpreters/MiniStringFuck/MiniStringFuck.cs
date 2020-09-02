using System;
using System.Collections.Generic;
using System.Linq;

namespace Codewars.EsolangInterpreters.MiniStringFuck
{
    public class MiniStringFuck
    {
        public static readonly Dictionary<char, Action<MiniStringFuckState>> Commands = new Dictionary<char, Action<MiniStringFuckState>>()
        {
            { '+', state => ++state.MemoryCell },
            { '.', state => state.Output.Add((char)state.MemoryCell) },
        };

        public static string MyFirstInterpreter(string code)
        {
            var state = new MiniStringFuckState(code);
            do
            {
                state.ProcessCommand(Commands);
                state.Move();
            } while (state.KeepProcessing);

            return state.Result;
        }
    }

    public class MiniStringFuckState
    {
        private List<char> Program { get; set; } = new List<char>();
        private int ProgramCounter { get; set; } = 0;
        
        public bool KeepProcessing { get; private set; } = true;
        public byte MemoryCell { get; set; } = 0;
        public List<char> Output { get; } = new List<char>();
        public string Result => string.Join(string.Empty, Output);

        public MiniStringFuckState(string code)
        {
            Program = code.ToList();
        }

        public void Move() => KeepProcessing = (++ProgramCounter < Program.Count);
        public void ProcessCommand(Dictionary<char, Action<MiniStringFuckState>> commands)
        {
            var command = GetCommand();
            if (commands.TryGetValue(command, out var func))
            {
                func(this);
            }
        }       

        private char GetCommand() => Program[ProgramCounter];
    }
}
