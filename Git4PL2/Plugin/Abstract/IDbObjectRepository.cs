using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    public interface IDbObjectRepository : IDbObject
    {
        void DirectoriesChecks();
        
        string DescriptionName { get; }

        string GetRawFilePath();

        string FileName { get; }

        string RepName { get;  }
    }
}
