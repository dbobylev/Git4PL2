using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Commands
{
    public class TextOperationsParametrs
    {
        public IDbObjectText DbObjectText {get; set;}

        public string StringParam { get; set; }
    }
}
