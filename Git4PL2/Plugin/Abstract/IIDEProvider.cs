﻿using Git4PL2.Plugin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    interface IIDEProvider
    {
        T GetDbObject<T>() where T : IDbObject;

        void SetStatusMessage(string text);

        string GetDatabaseConnection();

        bool SetText(string Text);
    }
}
