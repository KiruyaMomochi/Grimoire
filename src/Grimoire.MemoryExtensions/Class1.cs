using System;

namespace Grimoire.MemoryExtensions
{
    public static class Class1
    {
        public static void Main()
        {
        }
    }
}

namespace Grimoire.Memory
{
    public ref struct SpanSplitEnumerator<T> where T : System.IEquatable<T>
    {
        private readonly ReadOnlySpan<T> _buffer;

        private readonly ReadOnlySpan<T> _separators;
        private readonly T _separator;

        private readonly int _separatorLength;
        private readonly bool _splitOnSingleToken;

        private readonly bool _isInitialized;

        private int _startCurrent;
        private int _endCurrent;
        private int _startNext;

        public SpanSplitEnumerator<T> GetEnumerator()
        {
            return this;
        }

        public Range Current => new(_startCurrent, _endCurrent);

        public bool MoveNext()
        {
            if (!_isInitialized || _startNext > _buffer.Length)
            {
                return false;
            }

            var slice = _buffer[_startNext..];
            _startCurrent = _startNext;

            var separatorIndex = _splitOnSingleToken ? slice.IndexOf(_separator) : slice.IndexOf(_separators);
            var elementLength = separatorIndex != -1 ? separatorIndex : slice.Length;

            _endCurrent = _startCurrent + elementLength;
            _startNext = _endCurrent + _separatorLength;

            return true;
        }
    }
}