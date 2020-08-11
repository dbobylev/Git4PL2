using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Abstarct
{
    interface ICallbackManager
    {
        T GetDelegate<T>();
        void SetDelegate(int index, IntPtr function);
    }
}
