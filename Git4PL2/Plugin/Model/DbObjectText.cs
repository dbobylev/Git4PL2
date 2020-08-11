using Git4PL2.Abstarct;
using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Model
{
    class DbObjectText :DbObject, IDbObjectText
    {
        private IPlsqlCodeFormatter _PlsqlCodeFormatter;

        protected string _text;
        public string Text { get => _text; set => _text = value; }
        protected bool UTF8HaveBOM { get; set; }

        public DbObjectText(DbObject dbObj, string text) : base(dbObj)
        {
            Seri.Log.Here().Debug($"Создаём объект DbObjectText. text.length={text.Length}");

            DirectoriesChecks();

            Text = text;

            var FilePathParametr = NinjectCore.GetParameter("remoteFilePath", GetRawFilePath());
            _PlsqlCodeFormatter = NinjectCore.Get<IPlsqlCodeFormatter>(FilePathParametr);

            _PlsqlCodeFormatter.UpdateBeginOfText(ref _text, ObjectOwner, ObjectName);
            _PlsqlCodeFormatter.UpdateLastLines(ref _text);

            UTF8HaveBOM = _PlsqlCodeFormatter.IsBom();
            Seri.Log.Here().Verbose("UTF8HaveBOM={0}", UTF8HaveBOM.ToString());

            Seri.Log.Here().Verbose($"Конец конструктора DbObjectText");
        }

        public Encoding GetSaveEncoding()
        {
            Encoding EncodingToSave;

            switch ((eSaveEncodingType)Properties.Settings.Default.SaveEncodingType)
            {
                case eSaveEncodingType.UTF8: EncodingToSave = new UTF8Encoding(false); break;
                case eSaveEncodingType.UTF8_BOM: EncodingToSave = new UTF8Encoding(true); break;
                case eSaveEncodingType.DontChange: EncodingToSave = new UTF8Encoding(UTF8HaveBOM); break;
                default: EncodingToSave = new UTF8Encoding(false); break;
            }
            return EncodingToSave;
        }
    }
}
