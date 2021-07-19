using System;

namespace Grimoire.Explore.Parameter
{
    public class MessageParameterDescriptor
    {
        private static readonly bool[,] AssignableList = {
            {true, true, true},
            {false, true, true},
            {false, false, true}
        };

        public MessageParameterDescriptor(Range range, ReadOnlySpan<char> content)
        {
            Range = range;
            Content = content.ToString();
            Type = CheckType(content);
        }

        static ParameterType CheckType(ReadOnlySpan<char> span)
        {
            var state = ParameterType.None;
            if (span.Length == 0)
                return state;

            var idx = 0;
            var ch = span[idx];
            if (char.IsDigit(ch))
                state = ParameterType.Unsigned;
            else if (ch == '-')
                state = ParameterType.Signed;
            else
                return ParameterType.String;
            idx++;

            while (idx < span.Length)
            {
                if (!char.IsDigit(span[idx]))
                    return ParameterType.String;
                idx++;
            }

            return state;
        }
        
        public Range Range { get; }
        public string Content { get; }

        public ParameterType Type
        {
            get => _type;
            set
            {
                _type = value;
                _uintPossible = AssignableList[(int) Type, (int) ParameterType.Unsigned];
            }
        }

        public bool TryParseInt(out int result)
        {
            if (_intCache.HasValue)
            {
                result = _intCache.Value;
                return true;
            }
            
            _intPossible ??= AssignableList[(int) Type, (int) ParameterType.Signed];

            if (!_intPossible.Value)
            {
                result = 0;
                return false;
            }

            if (int.TryParse(Content, out result))
            {
                _intCache = result;
                return true;
            }

            _intPossible = false;
            return false;
        }
        
        public bool TryParseUint(out uint result)
        {
            _uintPossible ??= AssignableList[(int) Type, (int) ParameterType.Unsigned];

            if (!_uintPossible.Value)
            {
                result = 0;
                return false;
            }

            if (_uintCache.HasValue)
            {
                result = _uintCache.Value;
                return true;
            }

            if (uint.TryParse(Content, out result))
            {
                _uintCache = result;
                return true;
            }

            _uintPossible = false;
            return false;
        }
        
        private bool? _intPossible;
        private int? _intCache;
        
        private bool? _uintPossible;
        private uint? _uintCache;
        
        private ParameterType _type;

        public string ToString(string str) => $"{str[Range]}: {Type}";
        public override string ToString() => $"{Range}: {Type}";
    }
}