using System;
using System.Collections.Generic;

using ThinGin.Core.Common.Textures.Types;

namespace ThinGin.Core.Common.Textures
{
    public class TextureCache
    {
        #region Properties
        /// <summary>
        /// Maps pixel CRC hashes to the <see cref="Texture"/> instance holding the data.
        /// </summary>
        private readonly Dictionary<string, Texture> Cache = new Dictionary<string, Texture>();

        /// <summary>
        /// List of users for textures
        /// </summary>
        private readonly Dictionary<string, int> Users = new Dictionary<string, int>();
        #endregion

        #region Registration
        public bool TryRegister(string Identifier, Texture texture)
        {
            if (!Cache.ContainsKey(Identifier))
            {
                Cache.Add(Identifier, texture);
                Users.Add(Identifier, 0);
                return true;
            }

            return false;
        }

        private void Unregister(string identifier)
        {
            Cache.Remove(identifier);
            Users.Remove(identifier);
        }
        #endregion

        #region Referencing
        /// <summary>
        /// Registers a new user for the specified <paramref name="Identifier"/>
        /// </summary>
        /// <param name="Identifier"></param>
        /// <returns></returns>
        public bool TryReference(string Identifier, out Texture outTexture)
        {
            if (!Cache.ContainsKey(Identifier))
            {
                outTexture = null;
                return false;
            }

            if (!Users.ContainsKey(Identifier))
            {
                outTexture = null;
                return false;
            }

            Users[Identifier] += 1;
            outTexture = Cache[Identifier];
            return true;
        }

        internal void Dereference(string Identifier)
        {
            if (Users.ContainsKey(Identifier))
            {
                Users[Identifier] -= 1;

                if (Users[Identifier] <= 0)
                {
                    // Here we consider unloading the texture
                    if (Cache.TryGetValue(Identifier, out Texture texture))
                    {
                        // Any texture of priority zero or below shall be instantly unloaded once it is no longer referenced.
                        // Thus, only textures which the user gives a higher priority will persist.
                        if (texture.ResourcePriority <= 0)
                        {
                            Unregister(Identifier);
                        }
                    }
                }
            }
        }
        #endregion

        #region Lookups
        /// <summary>
        /// This is an internal function and should not be used by end users, the cache system operates through the <see cref="TextureProxy"/> objects
        /// </summary>
        internal WeakReference<Texture> Lookup(string Identifier)
        {
            if (Cache.TryGetValue(Identifier, out Texture outTexture))
            {
                return new WeakReference<Texture>(outTexture);
            }

            return null;
        }

        public bool IsCached(string Identifier)
        {
            return Cache.ContainsKey(Identifier);
        }
        #endregion
    }
}
