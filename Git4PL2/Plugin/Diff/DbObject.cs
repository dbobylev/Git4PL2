using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Diff
{
    class DbObject : IDbObject
    {
        public string ObjectOwner { get; protected set; }
        public string ObjectName { get; protected set; }
        public string ObjectType { get; protected set; }

        public DbObject(string Owner, string name, string type)
        {
            ObjectOwner = Owner.ToUpper();
            ObjectName = name.ToUpper();
            ObjectType = type.ToUpper();
        }

        public IDbObjectRepository GetObjectRepository()
        {
            return new DbObjectRepository(ObjectOwner, ObjectName, ObjectType);
        }

        public override string ToString()
        {
            return $"{ObjectType} {ObjectOwner}.{ObjectName}";
        }
    }
}
