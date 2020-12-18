using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day18
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.ToArray();
        }

        private readonly string[] _input;

        private abstract record Token(string S);
        private record ParenOpen() : Token("(");
        private record ParenClose() : Token(")");
        private record Number(long Value) : Token(Value.ToString());

        private abstract record Op(string S) : Token(S)
        {
            public abstract long F(long a, long b);
        }

        private record OpAdd() : Op("+")
        {
            public override long F(long a, long b) => a + b;
        }

        private record OpMul() : Op("*")
        {
            public override long F(long a, long b) => a * b;
        }

        private static IEnumerable<Token> Tokenize(string line)
        {
            var i = 0;
            while (i < line.Length)
            {
                var c = line[i];
                switch (c)
                {
                    case '(':
                        yield return new ParenOpen();
                        break;
                    case ')':
                        yield return new ParenClose();
                        break;
                    case >='0' and <= '9':
                        var n = 0;
                        do
                        {
                            n = n * 10 + (line[i] - '0');
                            i++;
                        } while (i < line.Length && line[i] >= '0' && line[i] <= '9');
                        i--;
                        yield return new Number(n);
                        break;
                    case '+':
                        yield return new OpAdd();
                        break;
                    case '*':
                        yield return new OpMul();
                        break;
                    case ' ':
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Invalid symbol");
                }
                i++;
            }
        }

        private static IEnumerable<Token> ToRpn(IEnumerable<Token> tokens, Func<Op, int> precedence)
        {
            var q = new Queue<Token>();
            var s = new Stack<Token>();

            foreach (var t in tokens)
            {
                switch (t)
                {
                    case Number:
                        q.Enqueue(t);
                        break;
                    case ParenOpen:
                        s.Push(t);
                        break;
                    case ParenClose:
                        do
                        {
                            var o = s.Pop();
                            if (o is ParenOpen)
                            {
                                break;
                            }
                            if (o is Op)
                            {
                                q.Enqueue(o);
                            }
                        } while (true);
                        break;
                    case Op o:
                        while (s.Any()
                            && s.Peek() is Op os
                            && precedence(o) <= precedence(os))
                        {
                            q.Enqueue(s.Pop());
                        }
                        s.Push(t);
                        break;
                    default:
                        throw new InvalidOperationException();
                };
            }

            while (s.Any())
            {
                q.Enqueue(s.Pop());
            }

            return q;
        }

        private static long EvalRpn(IEnumerable<Token> tokens)
        {
            var stack = new Stack<long>();
            foreach (var t in tokens)
            {
                switch (t)
                {
                    case Number n:
                        stack.Push(n.Value);
                        break;
                    case Op o:
                        var a = stack.Pop();
                        var b = stack.Pop();
                        stack.Push(o.F(a, b));
                        break;
                    default:
                        throw new InvalidOperationException();
                };
            }

            if (stack.Count != 1)
            {
                throw new InvalidOperationException("The stack should have only single item now");
            }

            return stack.Pop();
        }

        private static int AllOpsAreEqual(Op op) => 0;
        private static int AddBeforeMul(Op op) => 
            op switch
            {
                OpAdd => 1,
                OpMul => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(op)),
            };

        public static long Eval(string line) => EvalRpn(ToRpn(Tokenize(line), AllOpsAreEqual));
        public static long Eval2(string line) => EvalRpn(ToRpn(Tokenize(line), AddBeforeMul));

        public override long Solution1() => _input.Select(Eval).Sum();
        public override long Solution2() => _input.Select(Eval2).Sum();
    }
}
