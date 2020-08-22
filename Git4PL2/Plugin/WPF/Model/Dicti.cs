using Git4PL2.IDE.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.WPF.Model
{
    class Dicti
    {
        [SQLColumn("ISN")]
        public long Isn { get; set; }

        [SQLColumn("PARENTISN")]
        public long ParentIsn { get; set; }

        [SQLColumn("CODE")]
        public string Code { get; set; }

        [SQLColumn("SHORTNAME")]
        public string ShortName { get; set; }

        [SQLColumn("FULLNAME")]
        public string FullName { get; set; }

        [SQLColumn("CONSTNAME")]
        public string ConstName { get; set; }

        [SQLColumn("UPDATED")]
        public string Updated { get; set; }

        [SQLColumn("UPDATEDBY")]
        public string UpdatedBy { get; set; }

        [SQLColumn("ACTIVE")]
        public string Active { get; set; }

        [SQLColumn("LEVEL")]
        public int? Level { get; set; }
    }
}
