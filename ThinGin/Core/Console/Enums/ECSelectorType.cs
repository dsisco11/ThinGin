namespace ThinGin.Core.Console
{
    public enum ECSelectorType
    {
        /// <summary> Executes a single selector function on a dataset </summary>
        Simple,
        /// <summary> Executes a simple selector on a predefined target reference </summary>
        Reference,
        /// <summary> Comprised of multiple subselector objects which are all applied to the dataset and whose return values are all collected </summary>
        Compound,
        /// <summary> Acts as a filter over the dataset it is applied to </summary>
        Filter,
    }
}
