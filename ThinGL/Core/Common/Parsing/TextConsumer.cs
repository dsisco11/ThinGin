using System;

namespace ThinGin.Core.Common.Types
{
    public class TextConsumer : DataConsumer<char>
    {
        #region Constructors
        public TextConsumer(ReadOnlyMemory<char> Memory) : base(Memory)
        {
        }

        public TextConsumer(char[] Items) : base(Items)
        {
        }

        public TextConsumer(ReadOnlyMemory<char> Memory, char EOF_ITEM) : base(Memory, EOF_ITEM)
        {
        }

        public TextConsumer(char[] Items, char EOF_ITEM) : base(Items, EOF_ITEM)
        {
        }
        #endregion

        public bool Consume_Whitespace() => Consume_While(char.IsWhiteSpace);
    }
}
