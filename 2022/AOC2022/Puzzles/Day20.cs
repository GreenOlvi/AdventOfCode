using NUnit.Framework;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace AOC2022.Puzzles;

public class Day20 : CustomBaseDay
{
    private readonly int[] _input;

    public Day20()
    {
        _input = ReadLinesFromFile().Parse<int>().ToArray();
    }

    public Day20(IEnumerable<string> lines)
    {
        _input = lines.Parse<int>().ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        var list = new LinkedList<int>(_input);

        for (var i = 0; i < _input.Length; i++)
        {
            var n = list.GetNodeInOriginalOrder(i);
            n.Move(n.Value, list.Count);
        }

        var zero = list.FindNode(0);
        var result = new[] { 1000, 2000, 3000 }
            .Select(i => zero.GoNext(i % list.Count).Value)
            .Sum();

        return result.ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        const long key = 811589153L;
        var list = new LinkedList<long>(_input.Select(n => n * key));

        for (var j = 0; j < 10; j++)
        {
            for (var i = 0; i < _input.Length; i++)
            {
                var n = list.GetNodeInOriginalOrder(i);
                n.Move(n.Value, list.Count);
            }
        }

        var zero = list.FindNode(0);
        var result = new[] { 1000, 2000, 3000 }
            .Select(i => zero.GoNext(i % list.Count).Value)
            .Sum();

        return result.ToResult();
    }

    private class LinkedList<T> where T : notnull
    {
        public ListElement<T>? Head { get; }
        public int Count { get; }

        private readonly List<ListElement<T>> _nodes = new();

        public LinkedList(IEnumerable<T> values)
        {
            if (!values.Any())
            {
                return;
            }

            Head = new ListElement<T>(values.First());
            _nodes.Add(Head);

            var count = 1;
            var prev = Head;
            foreach (var value in values.Skip(1))
            {
                var e = new ListElement<T>(value)
                {
                    Prev = prev,
                };
                prev.Next = e;
                _nodes.Add(e);

                prev = e;
                count++;
            }

            Head.Prev = prev;
            Head.Prev.Next = Head;
            Count = count;
        }

        public ListElement<T> GetNodeInOriginalOrder(int i) => _nodes[i];

        public ListElement<T> FindNode(T value)
        {
            var node = Head;
            do
            {
                if (node!.Value.Equals(value))
                {
                    return node;
                }
                node = node.Next;
            } while (node != Head);

            throw new InvalidOperationException("Value not found");
        }

        public ReadOnlyCollection<T> State()
        {
            if (Head is null)
            {
                return Array.Empty<T>().AsReadOnly();
            }

            var list = new List<T>() { Head.Value };
            var head = Head;
            var node = head!.Next;
            while (node is not null && node != head)
            {
                list.Add(node.Value);
                node = node!.Next;
            }

            return list.AsReadOnly();
        }

        public record ListElement<TElement> where TElement : notnull
        {
            public TElement Value { get; init; }
            public ListElement<TElement>? Prev { get; set; }
            public ListElement<TElement>? Next { get; set; }

            public ListElement(TElement value)
            {
                Value = value;
            }

            public void Move(long count, int listLength)
            {
                var goNext = count > 0;
                count = goNext ? count : -count;
                var steps = count % (listLength - 1);

                if (steps == 0)
                {
                    return;
                }

                Unlink();
                if (goNext)
                {
                    var n = GoNext(steps);
                    InsertAfter(n!);
                }
                else
                {
                    var n = GoPrev(steps);
                    InsertAfter(n!.Prev!);
                }
            }

            public ListElement<TElement> GoNext(long count)
            {
                var n = this;
                for (var i = 0L; i < count; i++)
                {
                    n = n!.Next;
                }
                return n!;
            }

            private ListElement<TElement> GoPrev(long count)
            {
                var n = this;
                for (var i = 0L; i < count; i++)
                {
                    n = n!.Prev;
                }
                return n!;
            }

            private void Unlink()
            {
                Prev!.Next = Next;
                Next!.Prev = Prev;
            }

            private void InsertAfter(ListElement<TElement> e)
            {
                Next = e!.Next;
                Prev = e;
                e.Next!.Prev = this;
                e.Next = this;
            }

            public override string ToString() => $"({Prev!.Value}; {Value}; {Next!.Value})";
        }
    }
}
