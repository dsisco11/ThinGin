using ThinGin.Core.Console.Parsing;

namespace ThinGin.Core.Console.Commands.Selectors
{
    public delegate object SelectorResolver(ConsoleContext Context, CToken Value);
}
