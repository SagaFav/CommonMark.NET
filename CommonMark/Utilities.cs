﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CommonMark
{
    /// <summary>
    /// Reusable utility functions, not directly related to parsing or formatting data.
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Writes a warning to the Debug window.
        /// </summary>
        /// <param name="message">The message with optional formatting placeholders.</param>
        /// <param name="args">The arguments for the formatting placeholders.</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Warning(string message, params object[] args)
        {
            if (args != null && args.Length > 0)
                message = string.Format(message, args);

            System.Diagnostics.Debug.WriteLine(message, "Warning");
        }

        /// <summary>
        /// Converts TABs to spaces in the given string. Assumes that the string does not contain any newlines, or at most only a trailing newline.
        /// </summary>
        /// <param name="s">The source string.</param>
        /// <param name="tabSize">The size of the TAB (default 4 spaces).</param>
        /// <returns>A new string with TAB characters converted to spaces.</returns>
        public static string Untabify(string s, int tabSize = 4)
        {
            if (tabSize < 1 || tabSize > 8)
                throw new ArgumentOutOfRangeException("tabSize", "The value must be between 1 and 8 (inclusive).");

            if (s == null || s.Length == 0)
                return s;

            int step;
            StringBuilder sb = null;
            int realPos = 0;
            int lastPos = 0;
            int pos;

            while (-1 != (pos = s.IndexOf('\t', lastPos)))
            {
                realPos += pos - lastPos;

                if (sb == null)
                    sb = new StringBuilder(s.Length + 12);

                step = tabSize - (realPos % tabSize);
                realPos += step;
                sb.Append(s, lastPos, pos - lastPos);
                sb.Append(' ', step);
                lastPos = pos + 1;
            }

            if (sb == null)
                return s;

            sb.Append(s, lastPos, s.Length - lastPos);
            return sb.ToString();
        }

#if OptimizeFor45
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsAsciiSymbol(char c)
        {
            // char.IsSymbol also works with Unicode symbols that cannot be escaped based on the specification.
            return (c > ' ' && c < '0') || (c > '9' && c < 'A') || (c > 'Z' && c < 'a') || (c > 'z' && c < 127);
        }
    }
}
