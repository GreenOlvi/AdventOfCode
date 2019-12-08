using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle08
{
    public class LayeredImage
    {
        public LayeredImage(int[] input, int width, int height)
        {
            _data = input;
            LayerSize = width * height;
            LayerCount = input.Length / LayerSize;
        }

        private readonly int[] _data;
        public int LayerSize { get; }
        public int LayerCount { get; }

        public IEnumerable<int> Layer(int i) =>
            _data.Skip(i * LayerSize).Take(LayerSize);

        public IEnumerable<IEnumerable<int>> Layers() =>
            Enumerable.Range(0, LayerCount).Select(i => Layer(i));

        public int Checksum()
        {
            var minZeros = Layers()
                .Select((data, index) => (index, data.Count(x => x == 0)))
                .Aggregate((a, b) => a.Item2 <= b.Item2 ? a : b)
                .index;

            var layer = Layer(minZeros).ToArray();
            var ones = layer.Count(x => x == 1);
            var twos = layer.Count(x => x == 2);

            return ones * twos;
        }

        public int GetPixel(int layer, int index) =>
            _data[layer * LayerSize + index];

        private IEnumerable<int> StackedPixels(int index) =>
            Enumerable.Range(0, LayerCount).Select(l => GetPixel(l, index));

        public IEnumerable<int> Flatten() =>
            Enumerable.Range(0, LayerSize)
                .Select(i => StackedPixels(i).First(p => p != 2));
    }
}
