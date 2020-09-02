using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Codewars.EsolangInterpreters.PaintFuck
{
    public class PaintFuck
    {
        public static readonly Dictionary<char, Action<PaintFuckState>> Commands = new Dictionary<char, Action<PaintFuckState>>()
    {
        { 'n', state => state.UpdateDataPointer(state.DataPointer.X, state.DataPointer.Y - 1) },
        { 'e', state => state.UpdateDataPointer(state.DataPointer.X + 1, state.DataPointer.Y) },
        { 's', state => state.UpdateDataPointer(state.DataPointer.X, state.DataPointer.Y + 1) },
        { 'w', state => state.UpdateDataPointer(state.DataPointer.X - 1, state.DataPointer.Y) },
        { '*', state => state.CurrentCell = state.CurrentCell == 0 ? 1 : 0 },
        { '[', state => state.CurrentDirection = state.CurrentCell == 0 ? PaintFuckState.Direction.JumpForward : PaintFuckState.Direction.Right },
        { ']', state => state.CurrentDirection = state.CurrentCell != 0 ? PaintFuckState.Direction.JumpBackward : PaintFuckState.Direction.Right },
    };

        public static string Interpret(string code, int iterations, int width, int height)
        {
            var state = new PaintFuckState(code, iterations, width, height);
            while (state.KeepProcessing)
            {
                state.ProcessCommand(Commands);
                state.Move();
            }
            return state.Result;
        }

        public class PaintFuckState
        {
            public enum Direction { Right, JumpForward, JumpBackward }
            private List<char> Program { get; set; } = new List<char>();
            private int ProgramCounter { get; set; } = 0;
            private int Iterations { get; set; } = 0;

            public Point DataPointer { get; private set; } = new Point(0, 0);
            public bool KeepProcessing { get; private set; } = true;

            public Direction CurrentDirection { get; set; } = Direction.Right;
            public List<List<int>> Output { get; } = new List<List<int>>();
            public string Result => string.Join("\r\n", Output.Select(x => string.Join(string.Empty, x)));
            public int CurrentCell { get => Output[DataPointer.Y][DataPointer.X]; set => Output[DataPointer.Y][DataPointer.X] = value; }

            public PaintFuckState(string code, int iterations, int width, int height)
            {
                Program = code.ToList();
                Iterations = iterations;

                for (var i = 0; i < height; i++)
                {
                    Output.Add(new List<int>());
                    for (int j = 0; j < width; j++)
                    {
                        Output[i].Add(0);
                    }
                }

                KeepProcessing = Iterations > 0;
            }

            public void Move()
            {
                switch (CurrentDirection)
                {
                    case Direction.Right:
                        ++ProgramCounter;
                        break;
                    case Direction.JumpForward:
                    case Direction.JumpBackward:
                        var stack = new Stack<char>();
                        stack.Push(GetCommand());
                        while (stack.Any())
                        {
                            ProgramCounter += (CurrentDirection == Direction.JumpForward ? 1 : -1);
                            var current = stack.Peek();
                            var command = GetCommand();
                            if (current == '[' && command == ']')
                                stack.Pop();
                            else if (current == ']' && command == '[')
                                stack.Pop();
                            else if (command == '[' || command == ']')
                                stack.Push(command);
                        }
                        ++ProgramCounter;
                        break;
                }

                CurrentDirection = Direction.Right;
                KeepProcessing = ProgramCounter >= 0 && ProgramCounter < Program.Count && Iterations > 0;
            }

            private char GetCommand() => Program[ProgramCounter];
            public void ProcessCommand(Dictionary<char, Action<PaintFuckState>> commands)
            {
                var command = GetCommand();
                if (commands.TryGetValue(command, out var func))
                {
                    func(this);
                    --Iterations;
                }
            }

            public void UpdateDataPointer(int x, int y)
            {
                if (y < 0)
                    y = Output.Count - 1;
                y %= Output.Count;

                if (x < 0)
                    x = Output[y].Count - 1;
                x %= Output[y].Count;

                DataPointer = new Point(x, y);
            }
        }
    }
}
