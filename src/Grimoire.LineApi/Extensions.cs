using System;

namespace Grimoire.LineApi
{
    public static class Extensions
    {
        public static string ToUpperFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("There is not first letter");

            Span<char> a = stackalloc char[s.Length];
            s.AsSpan(1).CopyTo(a.Slice(1));
            a[0] = char.ToUpper(s[0]);
            return new string(a);
        }
    }
}
