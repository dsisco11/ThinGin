using System.Linq;

using ThinGin.Core.Common.Enums;

using OpenTK.Graphics.OpenGL;

namespace ThinGin.OpenGL.Common.Engine
{
    /// <summary>
    /// Represents state machine values for the rendering engine abstraction
    /// </summary>
    public class StateMachine
    {
        #region Values
        protected bool[] State = null;
        #endregion

        #region Constructors
        public StateMachine()
        {
            State = new bool[System.Enum.GetValues(typeof(EEngineCap)).Cast<int>().Max() + 1];

            foreach(EEngineCap cap in System.Enum.GetValues(typeof(EEngineCap)))
            {
                EnableCap glCap = Bridge.Translate(cap);
                this[cap] = GL.IsEnabled(glCap);
            }

        }
        #endregion

        #region Indexers
        public bool this[EEngineCap Capability]
        {
            get { return State[(uint)Capability]; }
            set { State[(uint)Capability] = value; }
        }
        #endregion

        #region Engine Capabilities

        /// <summary>
        /// Attempts to update the existing value of a given property and returns true if the value was changed
        /// </summary>
        public bool Try_Update(EEngineCap Capability, bool value)
        {
            if (!State[(int)Capability].Equals(value))
            {
                State[(int)Capability] = value;
                return true;
            }

            return false;
        }
        #endregion

    }
}
