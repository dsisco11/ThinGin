using System;

namespace ThinGin.Core.Console
{

    /// <summary>
    /// Represents a line of text in the virtual consoles log
    /// </summary>
    public class VConsoleLine
    {
        #region Properties
        private string contents;
        public readonly WeakReference<VirtualConsole> Console;
        #endregion

        #region Accessors
        public string Text { get => contents; }
        public WeakReference<VConsoleLine> Pointer => new WeakReference<VConsoleLine>(this);
        #endregion

        #region Events
        public event Action<VConsoleChangeArgs> OnChange;
        #endregion

        #region Constructors
        public VConsoleLine(VirtualConsole console, string text)
        {
            this.contents = text;
            Console = new WeakReference<VirtualConsole>(console);
        }
        #endregion

        #region Content Management

        internal struct StringReplaceContext
        {
            public string Original { get; set; }
            public string Insert { get; set; }
            public int Offset { get; set; }
            public int Length { get; set; }
        }

        /// <summary>
        /// Replaces a given range of the lines content with new text
        /// </summary>
        /// <returns>Success</returns>
        public bool Replace(Range range, string text)
        {
            var idx = range.GetOffsetAndLength(contents.Length);

            int newLen = contents.Length - idx.Length;
            newLen += text.Length;
            var replaceContext = new StringReplaceContext()
            {
                Original = this.contents,
                Insert = text,
                Offset = idx.Offset,
                Length = idx.Length
            };

            this.contents = string.Create(newLen, replaceContext, (Span<char> chars, StringReplaceContext state) =>
            {
                int pos = 0;
                state.Original.AsSpan().Slice(pos, state.Offset).CopyTo(chars);
                pos += state.Offset;

                state.Insert.AsSpan().CopyTo(chars.Slice(pos));
                var cSpan = chars.Slice(pos + state.Insert.Length);
                // Now we add the context length to our position so we correctly snip out that range from the original string
                state.Original.AsSpan().Slice(pos + state.Length).CopyTo(cSpan);
            });

            int changeStart = idx.Offset;
            int changeLength = Math.Max(idx.Length, text.Length);
            OnChange?.Invoke(new VConsoleChangeArgs(this, changeStart, changeLength));

            return true;
        }

        /// <summary>
        /// Appends new text to the lines contents
        /// </summary>
        /// <returns></returns>
        public bool Append(string text)
        {
            int changeStart = contents.Length;
            contents = string.Concat(contents, text);

            OnChange?.Invoke(new VConsoleChangeArgs(this, changeStart, text.Length));
            return true;
        }

        public bool Append(ReadOnlySpan<char> text)
        {
            int changeStart = contents.Length;
            contents = string.Concat(contents, text);

            OnChange?.Invoke(new VConsoleChangeArgs(this, changeStart, text.Length));
            return true;
        }
        #endregion

    }
}
