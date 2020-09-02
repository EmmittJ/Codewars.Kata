using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Codewars.BefungeInterpreter
{
    public class BefungeInterpreter
    {
        public static readonly Dictionary<char, Action<BefungeState>> Commands = new Dictionary<char, Action<BefungeState>>()
        {
            { '.', state => state.WriteResult(state.Pop().ToString()) },
            { ',', state => state.WriteResult(((char)state.Pop()).ToString()) },

            { '@', state => state.KeepProcessing = false },
            { '"', state => state.IsStringMode = !state.IsStringMode },
            { '#', state => state.Move() },
            { '$', state => state.Pop() },
            { '\\', state => { var a = state.Pop(); var b = state.Pop(); state.Push(a); state.Push(b); } },
            { ':', state => state.Push(state.Count == 0 ? 0 : state.Peek()) },
            { '!', state => state.Push(state.Pop() == 0 ? 1 : 0) },
            { '`', state => { var a = state.Pop(); var b = state.Pop(); state.Push(b > a ? 1 : 0); } },

            { '^', state => state.CurrentDirection = BefungeState.Direction.Up },
            { '>', state => state.CurrentDirection = BefungeState.Direction.Right },
            { 'v', state => state.CurrentDirection = BefungeState.Direction.Down },
            { '<', state => state.CurrentDirection = BefungeState.Direction.Left },
            { '?', state => state.CurrentDirection = (BefungeState.Direction)state.Random.Next(0, 4) },
            { '_', state => state.CurrentDirection = state.Pop() == 0 ? BefungeState.Direction.Right : BefungeState.Direction.Left },
            { '|', state => state.CurrentDirection = state.Pop() == 0 ? BefungeState.Direction.Down : BefungeState.Direction.Up },

            { '0', state => state.Push(0) },
            { '1', state => state.Push(1) },
            { '2', state => state.Push(2) },
            { '3', state => state.Push(3) },
            { '4', state => state.Push(4) },
            { '5', state => state.Push(5) },
            { '6', state => state.Push(6) },
            { '7', state => state.Push(7) },
            { '8', state => state.Push(8) },
            { '9', state => state.Push(9) },

            { 'p', state => state.SetCommand(state.Pop(), state.Pop(), (char)state.Pop()) },
            { 'g', state => state.Push(state.GetCommand(state.Pop(), state.Pop())) },

            { '+', state => { var a = state.Pop(); var b = state.Pop(); state.Push(a + b); } },
            { '*', state => { var a = state.Pop(); var b = state.Pop(); state.Push(a * b); } },
            { '-', state => { var a = state.Pop(); var b = state.Pop(); state.Push(b - a); } },
            { '/', state => { var a = state.Pop(); var b = state.Pop(); state.Push(a == 0 ? 0 : b / a); } },
            { '%', state => { var a = state.Pop(); var b = state.Pop(); state.Push(b % a); } },
        };

        public string Interpret(string code)
        {
            var state = new BefungeState(code);
            do
            {
                state.ProcessCommand(Commands);
                state.Move();
            } while (state.KeepProcessing);

            return state.Result;
        }
    }

    public class BefungeState
    {
        public enum Direction { Up, Right, Down, Left }
        private List<List<char>> Grid { get; } = new List<List<char>>();
        private Point Position { get; set; } = new Point();
        private Stack<int> Stack { get; } = new Stack<int>();
        private Queue<string> ResultQueue { get; } = new Queue<string>();

        public Random Random { get; } = new Random();
        public Direction CurrentDirection { get; set; } = Direction.Right;
        public string Result => string.Join(string.Empty, ResultQueue);
        public bool KeepProcessing { get; set; } = true;
        public bool IsStringMode { get; set; } = false;

        public BefungeState(string code)
        {
            var lines = code.Split('\n');
            var size = lines.OrderByDescending(str => str.Length).First().Length;
            foreach (var row in lines)
            {
                var items = row.ToList();
                while (items.Count < size)
                {
                    items.Add(' ');
                }
                Grid.Add(items);
            }
        }

        public char GetCommand(int y, int x)
        {
            return Grid[y][x];
        }

        public void SetCommand(int y, int x, char v)
        {
            Grid[y][x] = v;
        }

        private void UpdatePosition(int x, int y) => Position = new Point(x, y);
        public void Move()
        {
            switch (CurrentDirection)
            {
                case Direction.Up:
                    UpdatePosition(Position.X, Position.Y - 1 < 0 ? 0 : Position.Y - 1);
                    break;
                case Direction.Right:
                    UpdatePosition((Position.X + 1) % Grid[Position.Y].Count, Position.Y);
                    break;
                case Direction.Down:
                    UpdatePosition(Position.X, (Position.Y + 1) % Grid.Count);
                    break;
                case Direction.Left:
                    UpdatePosition(Position.X - 1 < 0 ? 0 : Position.X - 1, Position.Y);
                    break;
            }
        }

        public void WriteResult(string output) => ResultQueue.Enqueue(output);
        public void ProcessCommand(Dictionary<char, Action<BefungeState>> commands)
        {
            var command = GetCommand(Position.Y, Position.X);
            if (IsStringMode && command != '"')
            {
                Push(command);
                return;
            }

            if (commands.TryGetValue(command, out var func))
            {
                func(this);
            }
        }

        public int Pop() => Stack.Pop();
        public int Peek() => Stack.Peek();
        public void Push(int item) => Stack.Push(item);
        public int Count => Stack.Count;

    }
}
