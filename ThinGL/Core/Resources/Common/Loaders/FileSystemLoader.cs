using System.IO;

namespace ThinGin.Core.Resources.Common.Loaders
{
    public class FileSystemLoader : IResourceLoader
    {
        #region Properties
        /// <summary>
        /// The base directory of the loaders search path.
        /// </summary>
        public readonly string Directory;
        #endregion

        #region Constructors
        public FileSystemLoader(string Directory)
        {
            this.Directory = Directory;
        }
        #endregion

        #region IResourceLoader

        /// <summary>
        /// <inheritdoc cref="IResourceLoader.TryRead(string, out byte[])"/>
        /// </summary>
        public bool TryRead(string FullPath, out byte[] RawData)
        {
            // Never trust a user, check and make sure the provided path isnt relative...
            if (!Path.IsPathFullyQualified(FullPath))
            {
                // Nope, its relative, so we need to TRY and fully qualify it...
                string newPath = Path.GetFullPath(FullPath, Directory);
                if (File.Exists(newPath))
                {
                    FullPath = newPath;
                }
            }

            // Check if the file exists
            if (File.Exists(FullPath))
            {
                // Okay read all of its data into a buffer and lets get out of here, this place gives me the creeps...
                RawData = File.ReadAllBytes(FullPath);
                return true;
            }

            // Failure by default
            RawData = null;
            return false;
        }

        /// <summary>
        /// <inheritdoc cref="IResourceLoader.TrySearch(string, out string)"/>
        /// </summary>
        public bool TrySearch(string SearchStr, out string outFullPath)
        {
            // The user could have specified any kind of relative path and/or search string as the identifier
            // And that is valid, so...

            // First, combine the user specified search with our root search directory
            string combinedPath = Path.Combine(Directory, SearchStr);
            // Then, just in case it is still a relative path, resolve it to a full path.
            string fullpath = Path.GetFullPath(combinedPath);

            // Now lets get the separated filename/search & directory portions of the path
            string searchDir = Path.GetDirectoryName(fullpath);
            string searchFile = Path.GetFileName(fullpath);

            // Now perform the search on the directory...

            // Note: We want to enumerate the files here rather than calling GetFiles()
            // because, in the end, we are just going to use the first result if any and fetching all results will thus be slower (perhapse MUCH slower)
            var enumerator = System.IO.Directory.EnumerateFiles(searchDir, searchFile, SearchOption.TopDirectoryOnly);

            // Check if we have a result
            if (enumerator.GetEnumerator().MoveNext())
            {
                // Load the filedata
                outFullPath = enumerator.GetEnumerator().Current;
                return true;
            }

            // Failure by default
            outFullPath = string.Empty;
            return false;
        }
        #endregion
    }
}
