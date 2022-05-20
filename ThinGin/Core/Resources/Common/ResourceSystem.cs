using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ThinGin.Core.Resources.Common.Loaders;

namespace ThinGin.Core.Resources.Common
{
    /// <summary>
    /// Facilitates fetching data for resources. This system allows users to specify their own implementation that can account for the specific details of their own system (such as folder structure or reading from some kind of proprietary packed database).
    /// </summary>
    public static class ResourceSystem
    {
        #region Properties
        private static HashSet<IResourceLoader> Loaders = new HashSet<IResourceLoader>();

        public delegate bool ResourcePromise(out byte[] RawData);
        #endregion


        #region Loaders
        public static void Register(IResourceLoader Loader)
        {
            if (Loader is null)
            {
                throw new ArgumentNullException(nameof(Loader));
            }

            Loaders.Add(Loader);
        }

        public static void Unregister(IResourceLoader Loader)
        {
            if (Loader is null)
            {
                throw new ArgumentNullException(nameof(Loader));
            }

            Loaders.Remove(Loader);
        }
        #endregion

        #region Fetching
        /// <summary>
        /// <para>In the event that a loader returns false, the resourcing system will continue to query other registered loaders until it has found the resource or run out of options.</para>
        /// </summary>
        /// <param name="SearchStr">A string which identifies a resource, often a filename or fullpath however; this can also be a relative path or even a search query depending on what the underlying loader implementation supports.</param>
        public static byte[] Read(string SearchStr)
        {
            if (Loaders.Count > 0)
            {
                foreach (IResourceLoader loader in Loaders)
                {
                    if (loader.TryRead(SearchStr, out byte[] outFetchedData))
                    {
                        return outFetchedData;
                    }

                }
            }

            return null;
        }

        /// <summary>
        /// <para>In the event that a loader returns false, the resourcing system will continue to query other registered loaders until it has found the resource or run out of options.</para>
        /// </summary>
        /// <param name="SearchStr">A string which identifies a resource, often a filename or fullpath however; this can also be a relative path or even a search query depending on what the underlying loader implementation supports.</param>
        public static Task<byte[]> Read_Async(string SearchStr)
        {
            // This is bad async practice, i know
            return Task.Run(() =>
            {
                if (Loaders.Count > 0)
                {
                    foreach (IResourceLoader loader in Loaders)
                    {
                        if (loader.TryRead(SearchStr, out byte[] outFetchedData))
                        {
                            return outFetchedData;
                        }

                    }
                }

                return null;
            });
        }

        /// <summary>
        /// Quesries all registeres <see cref="IResourceLoader"/> instances and attempts to fetch the raw filedata for the resource indicated by the provided <paramref name="SearchStr"/>.
        /// </summary>
        /// <param name="SearchStr">A string which identifies a resource, often a filename or fullpath however; this can also be a relative path or even a search query depending on what the underlying loader implementation supports.</param>
        /// <param name="RawData">The resources raw, uninterpreted, binary data (eg; its file data)</param>
        /// <returns>Success</returns>
        public static bool TryRead(string SearchStr, out byte[] RawData)
        {
            if (Loaders.Count > 0)
            {
                foreach (IResourceLoader loader in Loaders)
                {
                    if (loader.TrySearch(SearchStr, out string outLocation))
                    {
                        if (loader.TryRead(outLocation, out RawData))
                        {
                            return true;
                        }
                    }
                }
            }

            RawData = null;
            return false;
        }
        #endregion

        #region Finding
        /// <summary>
        /// Queries all registered <see cref="IResourceLoader"/> instances and attempts to resolve the provided <paramref name="SearchStr"/> into a resource location.
        /// </summary>
        /// <param name="SearchStr">A string which identifies a resource, often a filename or fullpath however; this can also be a relative path or even a search query depending on what the underlying loader implementation supports.</param>
        public static string Search(string SearchStr)
        {
            if (Loaders.Count > 0)
            {
                foreach (IResourceLoader loader in Loaders)
                {
                    if (loader.TrySearch(SearchStr, out string outLocation))
                    {
                        return outLocation;
                    }

                }
            }

            return null;
        }

        /// <summary>
        /// Queries all registered <see cref="IResourceLoader"/> instances and attempts to resolve the provided <paramref name="SearchStr"/> into a resource location.
        /// </summary>
        /// <param name="SearchStr">A string which identifies a resource, often a filename or fullpath however; this can also be a relative path or even a search query depending on what the underlying loader implementation supports.</param>
        public static Task<string> Search_Async(string SearchStr)
        {
            // This is bad async practice, i know
            return Task.Run(() =>
            {
                if (Loaders.Count > 0)
                {
                    foreach (IResourceLoader loader in Loaders)
                    {
                        if (loader.TrySearch(SearchStr, out string outLocation))
                        {
                            return outLocation;
                        }

                    }
                }

                return null;
            });
        }
        #endregion

        #region Resolving
        /// <summary>
        /// Queries all registered <see cref="IResourceLoader"/> instances and attempts to find one which can fetch the resource specified by the <paramref name="SearchStr"/>.
        /// </summary>
        /// <param name="SearchStr">A string which identifies a resource, often a filename or fullpath however; this can also be a relative path or even a search query depending on what the underlying loader implementation supports.</param>
        /// <param name="Loader">The resource loader that claims to be capable of fetching the given resource.</param>
        /// <returns>Success</returns>
        public static bool TryGetLoader(string SearchStr, out IResourceLoader Loader)
        {
            if (Loaders.Count > 0)
            {
                foreach (IResourceLoader loader in Loaders)
                {
                    if (loader.TrySearch(SearchStr, out string outLocation))
                    {
                        Loader = loader;
                        return true;
                    }

                }
            }

            Loader = null;
            return false;
        }
        #endregion

        #region Promises
        /// <summary>
        /// Queries all registered <see cref="IResourceLoader"/> instances and attempts to resolve the provided <paramref name="SearchStr"/> into a promise for a resource fetch.
        /// </summary>
        /// <param name="SearchStr">A string which identifies a resource, often a filename or fullpath however; this can also be a relative path or even a search query depending on what the underlying loader implementation supports.</param>
        /// <param name="Promise">Either NULL or a promise (callback) function which can be invoked to retrieve the resource data.</param>
        /// <returns>Success</returns>
        public static bool TryGetPromise(string SearchStr, out ResourcePromise Promise)
        {
            if (Loaders.Count > 0)
            {
                foreach (IResourceLoader loader in Loaders)
                {
                    if (loader.TrySearch(SearchStr, out string outLocation))
                    {
                        Promise = (out byte[] outData) => loader.TryRead(outLocation, out outData);
                        return true;
                    }

                }
            }

            Promise = null;
            return false;
        }
        #endregion

    }
}
