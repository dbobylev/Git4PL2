﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    public interface IDbObjectText :IDbObjectRepository
    {
        string Text { get;  }

        Encoding GetSaveEncoding();
    }
}
