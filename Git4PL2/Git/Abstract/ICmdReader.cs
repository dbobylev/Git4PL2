using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git
{
    public interface ICmdReader
    {
        int ReadProcess(Process p);
        P GetResult<P>();
    }
}
