using Git4PL2.IDE.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.WPF.Model
{
    public class Ftoggle
    {
        [SQLColumn("ISN")]
        public long Isn { get; set; }

        [SQLColumn("CODE")]
        public string Code { get; set; }

        [SQLColumn("TASKID")]
        public string TaskID { get; set; }

        [SQLColumn("STATUS")]
        public string Status { get; set; }

        [SQLColumn("TEXT")]
        public string Text { get; set; }

        [SQLColumn("UPDATEDBY")]
        public string UpdatedBy { get; set; }

        [SQLColumn("UPDATED")]
        public string Updated { get; set; }

        public bool IsActive => Status == "A";
    }
}
