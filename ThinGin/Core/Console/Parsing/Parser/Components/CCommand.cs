using System.Text;

using ThinGin.Core.Common;
using ThinGin.Core.Console.Parsing;

namespace ThinGin.Core.Console
{
    /// <summary>
    /// Represents a virtual console command.
    /// </summary>
    public class CCommand : CComponent
    {
        #region Values
        public readonly string Name;
        public readonly CArgValue[] Arguments;
        #endregion

        /*
         * NOTES:
         * Commands are basically comprised of a name and list of components (arguments).
         * Components are representations of primitive object types that are used by the command system.
         * The command parser takes in primitive tokens and refines them into higher component abstractions to get as close to a useable value as possible.
         * Here are some of the component types I know are going to be needed:
         *  - Ident constants
         *  - Enum values
         *  - Numerics (integers/unsigned/floating point)
         *  - ObjectIDs
         *  - Functions
         *  - Selectors (@<ident>)
         *  - Strings
         */

        #region Constructors
        public CCommand(string name, params CArgValue[] arguments) : base(ECTokenType.Command)
        {
            Name = name;
            Arguments = arguments;
        }
        #endregion


        public override string Encode()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Name);
            sb.Append(UnicodeCommon.CHAR_SPACE);

            for (int i = 0; i < Arguments.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(UnicodeCommon.CHAR_SPACE);
                }

                sb.Append(Arguments[i].ToString());
            }

            sb.Append(UnicodeCommon.CHAR_SEMICOLON);

            return sb.ToString();
        }

    }
}
