using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codewars.EsolangInterpreters.Boolfuck
{
    public class Boolfuck
    {
        public static readonly Dictionary<char, Action<BoolfuckState>> Commands = new Dictionary<char, Action<BoolfuckState>>()
        {
            { '>', state =>  state.Data.MovePointer(BoolfuckData.PointerDirection.Right) },
            { '<', state =>  state.Data.MovePointer(BoolfuckData.PointerDirection.Left) },
            { '+', state =>  state.Data.CurrentCell = !state.Data.CurrentCell },
            { ',', state =>  state.Data.CurrentCell = state.Input.Read() },
            { ';', state =>  state.Output.Enqueue(state.Data.CurrentCell ? 1 : 0) },
            { '[', state => state.CurrentDirection = !state.Data.CurrentCell ? BoolfuckState.Direction.JumpForward : BoolfuckState.Direction.Right },
            { ']', state => state.CurrentDirection = state.Data.CurrentCell ? BoolfuckState.Direction.JumpBackward : BoolfuckState.Direction.Right },
        };

        public static string interpret(string code, string input)
        {
            var state = new BoolfuckState(code, input);
            while (state.KeepProcessing)
            {
                state.ProcessCommand(Commands);
                state.Move();
            }

            return state.Result;
        }


        public class BoolfuckState
        {
            public BoolfuckInput Input { get; private set; }
            public BoolfuckData Data { get; private set; }

            public enum Direction { Right, JumpForward, JumpBackward }
            public Direction CurrentDirection { get; set; } = Direction.Right;

            public Queue<int> Output { get; } = new Queue<int>();
            public bool KeepProcessing { get; private set; } = true;

            private List<char> Program { get; set; } = new List<char>();
            private int ProgramCounter { get; set; } = 0;

            public BoolfuckState(string code, string input)
            {
                Program = code.Where(c => Commands.Keys.Contains(c)).ToList();
                Input = new BoolfuckInput(input);
                Data = new BoolfuckData();

                KeepProcessing = Program.Count > 0;
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
                        CurrentDirection = Direction.Right;
                        break;
                }

                KeepProcessing = ProgramCounter < Program.Count;
            }

            private char GetCommand() => Program[ProgramCounter];
            public void ProcessCommand(Dictionary<char, Action<BoolfuckState>> commands)
            {
                var command = GetCommand();
                if (commands.TryGetValue(command, out var func))
                {
                    func(this);
                }
            }

            public string Result
            {
                get
                {
                    var result = new StringBuilder();
                    var temp = string.Empty;
                    while (Output.Count > 0)
                    {
                        temp += Output.Dequeue().ToString();
                        if (temp.Length == 8)
                        {
                            result.Append((char)Convert.ToByte(GetValue(temp), 2));
                            temp = string.Empty;
                        }
                    }

                    if (temp.Length > 0)
                    {
                        result.Append((char)Convert.ToByte(GetValue(temp), 2));
                    }
                    return result.ToString();
                }
            }

            private string GetValue(string value)
            {
                value = value.PadRight(8, '0');
                if (BitConverter.IsLittleEndian)
                    return string.Join(string.Empty, value.Reverse());
                return value;
            }
        }

        public class BoolfuckInput
        {
            private BitArray Input { get; set; } = new BitArray(0, false);

            public BoolfuckInput(string input)
            {
                Input = new BitArray(input.Select(x =>
                {
                    var bytes = BitConverter.GetBytes(x);
                    if (!BitConverter.IsLittleEndian) Array.Reverse(bytes);
                    return bytes.First();
                }).ToArray());
            }

            public bool Read()
            {
                if (Input.Length == 0) return false;

                var value = Input[0];

                var src = new bool[Input.Count];
                var dst = new bool[Input.Count - 1];
                Input.CopyTo(src, 0);
                Array.Copy(src, 1, dst, 0, dst.Length);
                Input = new BitArray(dst);

                return value;
            }
        }

        public class BoolfuckData
        {
            private BitArray BitArray { get; set; } = new BitArray(1, false);
            private int Pointer { get; set; } = 0;

            public bool CurrentCell { get => BitArray[Pointer]; set => BitArray[Pointer] = value; }
            public enum PointerDirection { Left, Right }

            public void MovePointer(PointerDirection direction)
            {
                switch (direction)
                {
                    case PointerDirection.Left:
                        if (--Pointer < 0)
                        {
                            Prepend(false);
                            ++Pointer;
                        }
                        break;
                    case PointerDirection.Right:
                        if (++Pointer >= BitArray.Count)
                        {
                            Append(false);
                        }
                        break;
                }
            }

            public void Prepend(bool bit)
            {
                var temp = new bool[BitArray.Count + 1];
                temp[0] = bit;
                BitArray.CopyTo(temp, 1);
                BitArray = new BitArray(temp);
            }

            public void Append(bool bit)
            {
                var temp = new bool[BitArray.Count + 1];
                BitArray.CopyTo(temp, 0);
                temp[BitArray.Count] = bit;
                BitArray = new BitArray(temp);
            }
        }
    }
}
