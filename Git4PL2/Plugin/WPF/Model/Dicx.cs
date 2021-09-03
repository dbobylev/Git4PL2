using Git4PL2.IDE.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.WPF.Model
{
    class Dicx
    {
        [SQLColumn("CLASSISNNAME")]
        public string ClassIsnName { get; set; }

        [SQLColumn("CLASSISN1NAME")]
        public string ClassIsn1Name { get; set; }

        [SQLColumn("CLASSISN2NAME")]
        public string ClassIsn2Name { get; set; }

        [SQLColumn("CLASSISN3NAME")]
        public string ClassIsn3Name { get; set; }

        [SQLColumn("FILTERISNNAME")]
        public string FilterIsnName { get; set; }

        [SQLColumn("CLASSISN4NAME")]
        public string ClassIsn4Name { get; set; }

        [SQLColumn("CLASSISN5NAME")]
        public string ClassIsn5Name { get; set; }

        [SQLColumn("ISN")]
        public long? Isn { get; set; }

        [SQLColumn("CLASSISN")]
        public long? ClassIsn { get; set; }

        [SQLColumn("CLASSISN1")]
        public long? ClassIsn1 { get; set; }

        [SQLColumn("CLASSISN2")]
        public long? ClassIsn2 { get; set; }

        [SQLColumn("CLASSISN3")]
        public long? ClassIsn3 { get; set; }

        [SQLColumn("FILTERISN")]
        public long? FilterIsn { get; set; }

        [SQLColumn("CLASSISN4")]
        public long? ClassIsn4 { get; set; }

        [SQLColumn("CLASSISN5")]
        public long? ClassIsn5 { get; set; }

        [SQLColumn("UPDATED")]
        public string Updated { get; set; }

        [SQLColumn("UPDATEDBY")]
        public string UpdatedBy { get; set; }

    }
}
