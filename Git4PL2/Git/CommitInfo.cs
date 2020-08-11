using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git
{
    public class CommitInfo
    {
        public string Author { get; private set; }
        public string EMail { get; private set; }
        public string SHA { get; private set; }
        public DateTime Date { get; private set; }

        public CommitInfo()
        {

        }

        public CommitInfo(string sha, string author, string email, DateTime date)
        {
            Author = author;
            SHA = sha;
            EMail = email;
            Date = date;
        }

        public override string ToString()
        {
            return $"SHA: {SHA}\nAuthor: {Author}\nEMail: {EMail}\nDate: {Date}";
        }
    }
}
