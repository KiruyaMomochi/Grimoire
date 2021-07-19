using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Grimoire.Explore.Collections;
using Grimoire.Explore.Common;

namespace Grimoire.Explore.Parameter
{
    public class MessageParametersDescriptor
    {
        private const int StackallocIntBufferSizeLimit = 128;

        public IList<MessageParameterDescriptor> ParametersDescriptors { get; set; }

        public int Count => ParametersDescriptors.Count;

        public MessageParametersDescriptor(ReadOnlySpan<char> span, int entriesMaxParametersCount)
        {
            if (entriesMaxParametersCount == 0)
            {
                ParametersDescriptors = ImmutableList<MessageParameterDescriptor>.Empty;
                return;
            }

            var sepListBuilder = new ValueListBuilder<int>(stackalloc int[StackallocIntBufferSizeLimit]);
            MakeSeparatorList(span, ref sepListBuilder);
            ParametersDescriptors = new List<MessageParameterDescriptor>();

            var i = 0;
            foreach (var s in new SpanSplitEnumerator(span, sepListBuilder.AsSpan()))
            {
                if (i >= entriesMaxParametersCount) break;
                i++;

                ParametersDescriptors.Add(new MessageParameterDescriptor(s, span[s]));
            }
        }
        
        internal static void MakeSeparatorList(ReadOnlySpan<char> span, ref ValueListBuilder<int> sepListBuilder)
        {
            var sep = " 　\n".AsSpan();
            var idx = span.IndexOfAny(sep);

            while (idx != -1)
            {
                sepListBuilder.Append(idx);
                span = span[(idx + 1)..];
                idx = span.IndexOfAny(sep);
            }
        }
        
        public bool TryBuildParameters(IList<ParameterType> parameterTypes, object[] parameters)
        {
            var i = 0;
            for (; i < parameterTypes.Count; i++)
            {
                switch (parameterTypes[i])
                {
                    case ParameterType.Signed:
                        if (!ParametersDescriptors[i].TryParseInt(out var intVar))
                            return false;
                        parameters[i] = intVar;
                        break;
                    case ParameterType.Unsigned:
                        if (!ParametersDescriptors[i].TryParseUint(out var uintVar))
                            return false;
                        parameters[i] = uintVar;
                        break;
                    case ParameterType.String:
                        parameters[i] = ParametersDescriptors[i].Content;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return true;
        }

        public string BuildRemainParameters(ReadOnlySpan<char> args, int parametersLength)
        {
            if (parametersLength == 0)
                return args.Trim().ToString();
            
            if (parametersLength < ParametersDescriptors.Count)
            {
                var lastStart = ParametersDescriptors[parametersLength].Range.Start;
                return args[lastStart..].ToString();
            }

            if (parametersLength > ParametersDescriptors.Count)
                throw new NotSupportedException("Impossible");

            var lastEnd = ParametersDescriptors[^1].Range.End;
            return args[lastEnd..].Trim().ToString();
        }

        public string ToString(string str)
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.AppendJoin(", ", ParametersDescriptors.Select(x => x.ToString(str)));
            sb.Append(')');

            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.AppendJoin(", ", ParametersDescriptors);
            sb.Append(')');

            return sb.ToString();
        }
    }
}