using System;

namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IEngineObservable
    {
        event EventHandler OnChanged;
    }
}
