namespace ThinGin.Core.Resources.Common.Loaders
{
    public interface IResourceLoader
    {
        /// <summary>
        /// Attempts to read the data of the resource specified by the <paramref name="FullPath"/>.
        /// <para>In the event that the loader returns false, the resourcing system will continue to query other registered loaders until it has found the resource or run out of options.</para>
        /// </summary>
        /// <param name="FullPath">A string which uniquely identifies a resource, a fullpath.</param>
        /// <param name="RawData">The resources raw, uninterpreted, binary data (eg; its file data)</param>
        /// <returns>True if successful, false otherwise</returns>
        bool TryRead(string FullPath, out byte[] RawData);

        /// <summary>
        /// Attempts to locate the resource specified by the <paramref name="SearchStr"/>.
        /// </summary>
        /// <param name="SearchStr">A string which identifies a resource, often a filename or fullpath however; this can also be a relative path or even a search query depending on what the underlying loader implementation supports.</param>
        /// <param name="FullPath">The fully resolved path of the resource.</param>
        /// <returns>True if successful, false otherwise</returns>
        bool TrySearch(string SearchStr, out string FullPath);
    }
}
