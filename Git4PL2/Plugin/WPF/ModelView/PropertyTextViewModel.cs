﻿using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.WPF.ModelView
{
    class PropertyTextViewModel :PropertyChangedBase
    {
        IPluginParameter _PluginParameter;




        public PropertyTextViewModel(IPluginParameter parameter)
        {
            _PluginParameter = parameter;
        }
    }
}