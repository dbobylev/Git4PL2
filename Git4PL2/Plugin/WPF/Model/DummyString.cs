﻿using Git4PL2.IDE.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.WPF.Model
{
    class DummyString
    {
        [SQLColumn("DUMMY")]
        public string Value { get; set; }
    }
}
