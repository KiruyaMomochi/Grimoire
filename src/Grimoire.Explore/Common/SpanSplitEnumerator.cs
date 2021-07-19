using System;

namespace Grimoire.Explore.Common
{
    public ref struct SpanSplitEnumerator
    {
        private readonly ReadOnlySpan<int> _sepList;
        private readonly ReadOnlySpan<char> _buffer;

        private ReadOnlySpan<int>.Enumerator _enumerator;
        private int _currentBegin, _currentEnd;

        internal SpanSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<int> sepList)
        {
            _buffer = span;
            _sepList = sepList;
            _currentBegin = 0;
            _currentEnd = -1;
            _enumerator = sepList.GetEnumerator();
        }

        public SpanSplitEnumerator GetEnumerator() => this;

        public Range Current => new(_currentBegin, _currentEnd);

        public bool MoveNext()
        {
            if (_currentEnd >= _buffer.Length)
                return false;

            if (!_enumerator.MoveNext())
            {
                _currentBegin = _currentEnd + 1;
                _currentEnd = _buffer.Length;
                return _currentBegin != _currentEnd;
            }

            _currentBegin = _currentEnd + 1;
            _currentEnd = _currentBegin + _enumerator.Current;

            return _currentBegin != _currentEnd || MoveNext();
        }
    }
}