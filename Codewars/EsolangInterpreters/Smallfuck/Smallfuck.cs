using System;
using System.Collections.Generic;
using System.Linq;

namespace Codewars.EsolangInterpreters.Smallfuck
{
    public class Smallfuck
    {
        public static readonly Dictionary<char, Action<SmallfuckState>> Commands = new Dictionary<char, Action<SmallfuckState>>()
        {
            { '>', state => ++state.TapeCounter },
            { '<', state => --state.TapeCounter },
            { '*', state => state.CurrentCell = state.CurrentCell == 0 ? 1 : 0 },
            { '[', state => state.CurrentDirection = state.CurrentCell == 0 ? SmallfuckState.Direction.JumpForward : SmallfuckState.Direction.Right },
            { ']', state => state.CurrentDirection = state.CurrentCell != 0 ? SmallfuckState.Direction.JumpBackward : SmallfuckState.Direction.Right },
        };

        public static string Interpreter(string code, string tape)
        {
            var state = new SmallfuckState(code, tape);
            do
            {
                state.ProcessCommand(Commands);
                state.Move();
            } while (state.KeepProcessing);

            return state.Result;
        }

        public class SmallfuckState
        {
            public enum Direction { Right, JumpForward, JumpBackward }
            private List<char> Program { get; set; } = new List<char>();
            private int ProgramCounter { get; set; } = 0;

            public int TapeCounter { get; set; } = 0;
            public Direction CurrentDirection { get; set; } = Direction.Right;
            public bool KeepProcessing { get; private set; } = true;
            public List<int> Output { get; } = new List<int>();
            public string Result => string.Join(string.Empty, Output);
            public int CurrentCell { get => Output[TapeCounter]; set => Output[TapeCounter] = value; }

            public SmallfuckState(string code, string tape)
            {
                Program = code.ToList();
                Output = tape.Select(x => int.Parse(x.ToString())).ToList();
            }

            public void Move()
            {
                var stack = new Stack<char>();
                switch (CurrentDirection)
                {
                    case Direction.Right:
                        ++ProgramCounter;
                        break;
                    case Direction.JumpForward:
                    case Direction.JumpBackward:
                        stack.Push(GetCommand());
                        while (stack.Any())
                        {
                            ProgramCounter = ProgramCounter + (CurrentDirection == Direction.JumpForward ? 1 : -1);
                            var current = stack.Peek();
                            var command = GetCommand();
                            if (current == '[' && command == ']')
                                stack.Pop();
                            else if (current == ']' && command == '[')
                                stack.Pop();
                            else if (command == '[' || command == ']') 
                                stack.Push(command);
                        }
                        break;
                }

                KeepProcessing = ProgramCounter < Program.Count && TapeCounter >= 0 && TapeCounter < Output.Count;
            }

            private char GetCommand() => Program[ProgramCounter];
            public void ProcessCommand(Dictionary<char, Action<SmallfuckState>> commands)
            {
                var command = GetCommand();
                if (commands.TryGetValue(command, out var func))
                {
                    func(this);
                }
            }

        }
    }
}
