using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using ThinGin.Core.Console.Commands;

namespace ThinGin.Core.Console
{
    /// <summary>
    /// Provides a definition for a virtual console command
    /// </summary>
    public class VCommandDef
    {
        #region Properties
        public readonly string Name;
        public readonly MethodInfo[] Handlers;
        #endregion

        #region Constructors
        public VCommandDef(string name, VCommand command)
        {
            Name = name;
            var ty = command.GetType();
            var methods = ty.GetMethods(BindingFlags.Static | BindingFlags.Public);
            var handlers = from m in methods where (m.GetCustomAttribute<CommandHandler>() is object) select m;

            Handlers = handlers.ToArray();
        }
        #endregion

        #region Handler Matching
        public MethodInfo[] Find_Handler(CArgValue[] Args)
        {
            var validHandlers = from handler in Handlers
                                where (handler.GetParameters().Length == Args.Length) 
                                select handler;

            var matches = new LinkedList<MethodInfo>();

            foreach (MethodInfo handler in validHandlers)
            {
                bool is_match = true;
                foreach (ParameterInfo parameter in handler.GetParameters())
                {
                    var arg = Args[parameter.Position];

                    if (!arg.Is_Match(parameter.ParameterType))
                    {
                        is_match = false;
                        break;
                    }
                }

                if (is_match)
                {
                    matches.AddLast(handler);
                }
            }

            return matches.ToArray();
        }
        #endregion

    }
}
