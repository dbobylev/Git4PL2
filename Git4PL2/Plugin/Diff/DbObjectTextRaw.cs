using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Diff
{
    class DbObjectTextRaw: DbObjectRepository, IDbObjectText
    {
        protected string _text;
        public string Text { get => _text; set => _text = value; }

        public DbObjectTextRaw(DbObjectRepository dbObj, string text) : base(dbObj)
        {
            Seri.Log.Here().Debug($"Создаём объект DbObjectTextRaw. text.length={text.Length}");

            DirectoriesChecks();
            Text = text;

            Seri.Log.Here().Verbose($"Конец конструктора DbObjectTextRaw");
        }

        public Encoding GetSaveEncoding()
        {
            return new UTF8Encoding(false);
        }
    }
}
