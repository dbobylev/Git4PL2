﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Abstarct
{
    interface IMenuItem
    {
        int Index { get; }

        string MenuName { get; }

        Bitmap Icon { get; }

        void Click();
    }
}
