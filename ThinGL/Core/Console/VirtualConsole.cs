using System;
using System.Collections.Generic;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Types;
using ThinGin.Core.Console.Commands.Selectors;
using ThinGin.Core.Console.Parsing;

namespace ThinGin.Core.Console
{
    /// <summary>
    /// Implements a virtual console for the engine.
    /// </summary>
    public class VirtualConsole
    {
        #region Constants
        const string VCONSOLE_LOG_HEADER = "[VConsole]";
        #endregion

        #region Values
        WeakReference<IEngine> _engineRef;
        /// <summary>
        /// Dictionary of registered commands
        /// </summary>
        private Dictionary<string, VCommandDef> commands;

        private Dictionary<string, SelectorResolver> resolvers;

        /// <summary>
        /// a FILO list of all console text
        /// </summary>
        private LinkedList<VConsoleLine> _log = new LinkedList<VConsoleLine>();
        #endregion

        #region Accessors
        public IEngine Engine => _engineRef.TryGetTarget(out var outRef) ? outRef : null;
        public IReadOnlyDictionary<string, VCommandDef> Commands => this.commands;
        public IEnumerator<VConsoleLine> Log => _log.GetEnumerator();
        public ConsoleContext Context => new ConsoleContext(this);
        #endregion

        #region Events
        public event Action<VirtualConsole> OnUpdate;
        public event VConsoleChangeDelegate OnSubUpdate;
        #endregion

        #region Constructors
        public VirtualConsole()
        {
            commands = new Dictionary<string, VCommandDef>();
        }
        #endregion

        #region Registration
        /// <summary>
        /// Registers a new command
        /// </summary>
        /// <returns></returns>
        public bool Register(VCommandDef def)
        {
            if (commands.TryGetValue(def.Name, out _))
            {
                System.Diagnostics.Trace.TraceWarning($"{VCONSOLE_LOG_HEADER} Attempt to register new command to preexisting name! ({def})");
                return false;
            }

            if (!this.commands.TryAdd(def.Name, def))
            {
                System.Diagnostics.Trace.TraceWarning($"{VCONSOLE_LOG_HEADER} Failed to register new console command! (reason: unknown) ({def})");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Registers a new command
        /// </summary>
        /// <returns></returns>
        public bool Register(string RefName, SelectorResolver solver)
        {
            if (resolvers.TryGetValue(RefName, out _))
            {
                System.Diagnostics.Trace.TraceWarning($"{VCONSOLE_LOG_HEADER} Attempt to register new selector resolver to preexisting name! ({RefName})");
                return false;
            }

            if (!resolvers.TryAdd(RefName, solver))
            {
                System.Diagnostics.Trace.TraceWarning($"{VCONSOLE_LOG_HEADER} Failed to register new console selector resolver! (reason: unknown) ({RefName})");
                return false;
            }

            return true;
        }
        #endregion

        #region Management
        /// <summary>
        /// Clears all lines from the console
        /// </summary>
        /// <returns></returns>
        public bool Clear()
        {
            _log.Clear();

            return true;
        }

        private void line_OnChange(VConsoleChangeArgs Args)
        {
            OnSubUpdate?.Invoke(this, Args);
        }

        protected VConsoleLine _create_line()
        {
            var cline = _get_active_line();
            if (cline is object)
            {
                _finalize_line(cline);
            }

            var newline = new VConsoleLine(this, string.Empty);
            _log.AddFirst(newline);
            newline.OnChange += line_OnChange;

            OnUpdate?.Invoke(this);
            return newline;
        }
        /// <summary>
        /// Finalized the contents of console line as it is pushed out of the active state by an incoming line.
        /// </summary>
        /// <param name="Line"></param>
        protected void _finalize_line(VConsoleLine Line)
        {
#if DEBUG
            System.Diagnostics.Trace.TraceInformation($"[VirtualConsole] {Line.Text}");
#endif
        }
        protected VConsoleLine _get_active_line() => _log.First.Value;
        protected VConsoleLine _get_or_create_active_line() => _get_active_line() ?? _create_line();
        #endregion

        #region Output
        /// <summary>
        /// Appends text to the current log line
        /// </summary>
        /// <returns>success</returns>
        public bool Output(string TextStr)
        {
            if (string.IsNullOrWhiteSpace(TextStr))
            {
                return false;
            }

            // we want to output lines according to any line endings given within the text
            var strm = new DataConsumer<char>(TextStr.AsMemory());
            while (!strm.atEnd)
            {
                //if (!strm.Consume_While((c) => c != '\n', out ReadOnlySpan<char> outConsumed))
                if (!strm.Consume_While(c => !char.IsSeparator(c), out ReadOnlySpan<char> outConsumed))
                    break;

                _get_or_create_active_line().Append(outConsumed);
                strm.Consume(1);// consume the newline char
            }

            return true;
        }

        /// <summary>
        /// Outputs the given text on a new line
        /// </summary>
        /// <returns>success</returns>
        public bool OutputLine(string TextStr)
        {
            _create_line();
            return Output(TextStr);
        }
        #endregion

        #region Console Command Management
        protected SelectorResolver get_resolver(string Name)
        {
            if (resolvers.TryGetValue(Name, out SelectorResolver solver))
            {
                return solver;
            }

            return null;
        }

        protected VCommandDef _get_def(string cmdName)
        {
            if (commands.TryGetValue(cmdName, out VCommandDef outDef))
            {
                return outDef;
            }

            return null;
        }

        public IEnumerable<object> Select(CReference reference)
        {
            string RefName = reference.Name.Encode();

            var solver = get_resolver(RefName);
            object res = solver.Invoke(this.Context, reference.Value);

            if (res is null)
                return null;
            else if (res is IEnumerable<object> eRes)
                return eRes;
            else
                return new[] { res };
        }

        public bool Execute(CCommand Cmd, out IEnumerable<object> Result)
        {
            OutputLine(string.Concat("> ", Cmd));

            var def = _get_def(Cmd.Name);
            var handlers = def.Find_Handler(Cmd.Arguments);

            if (handlers.Length > 0)
            {
                OutputLine($"{VCONSOLE_LOG_HEADER} command arguments results in ambiguity among defined handlers, unable to determine which handler method to execute!");
                Result = null;
                return false;
            }
            else if (handlers.Length <= 0)
            {
                OutputLine($"{VCONSOLE_LOG_HEADER} cannot find matching command handler for the given arguments!");
                Result = null;
                return false;
            }
            else
            {
                var handler = handlers[0];
                var paramsList = handler.GetParameters();
                var values = new object[Cmd.Arguments.Length];

                for (int i = 0; i < Cmd.Arguments.Length; i++)
                {
                    values[i] = Cmd.Arguments[i].Resolve(Context, paramsList[i].DefaultValue);
                }

                object res = handler.Invoke(null, values);
                if (res is null)
                {
                    Result = null;
                }
                else if (res is IEnumerable<object> eRes)
                {
                    Result = eRes;
                }

                Result = new[] { res };
            }

            return true;
        }
        #endregion

        #region Input
        /// <summary>
        /// Processes a user input string for the console
        /// </summary>
        /// <param name="InputStr"></param>
        /// <returns></returns>
        public bool Input(string InputStr)
        {
            var Parser = new CommandParser(InputStr.AsMemory());
            var cmdList = Parser.Parse_Command_List();

            foreach(CCommand cmd in cmdList)
            {
                if (!Execute(cmd, out IEnumerable<object> Results))
                {
                    return false;
                }

                if (Results is object)
                {
                    foreach(object o in Results)
                    {
                        OutputLine(o.ToString());
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
