using System;
using System.Collections.Generic;

using ThinGin.Core.World.Objects;

namespace ThinGin.Core.World
{
    public class WorldManager
    {
        #region Values
        internal UInt64 _objectid_seq = 0;
        private Dictionary<UInt64, ObjectID> objects = new Dictionary<UInt64, ObjectID>();
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public WorldManager()
        {
        }
        #endregion


        #region ObjectID Management
        public ObjectID Get_ObjectByID(UInt64 rawid)
        {
            if (objects.TryGetValue(rawid, out ObjectID outID))
            {
                return outID;
            }

            return null;
        }

        public ObjectID Register(WObject obj)
        {
            var id = new ObjectID(this, obj);
            if (objects.TryAdd(id.Value, id))
            {
                return id;
            }

            return null;
        }


        public bool Unregister(ObjectID id)
        {
            if (objects.Remove(id.Value))
            {
                return true;
            }

            return false;
        }
        #endregion


        #region Ticking
        public void Tick(double time)
        {
            foreach (var kvp in objects)
            {
                kvp.Value.Object.Tick(time);
            }
        }
        #endregion
    }
}
