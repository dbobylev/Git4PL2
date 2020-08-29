using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Diff
{
    static class Helper
    {
        public static readonly Dictionary<eDbObjectType, string> FileExtension = new Dictionary<eDbObjectType, string>()
        {
            { eDbObjectType.FUNCTION, "FNC" },
            { eDbObjectType.PROCEDURE, "PRC" },
            { eDbObjectType.PACKAGEBODY, "BDY" },
            { eDbObjectType.PACKAGE, "SPC" },
            { eDbObjectType.TRIGGER, "TRG" },
            { eDbObjectType.VIEW, "VWS" },
            { eDbObjectType.TYPE, "TPS" },
            { eDbObjectType.TYPEBODY, "TPB" }
        };

        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string[] GetTypeDescriptions(Type value)
        {
            return Enum.GetNames(value).Select(x => (Enum.Parse(value, x) as Enum).GetEnumDescription()).ToArray();
        }

        public static string GetCaseSensitiveFolderName(string path)
        {
            string ParentFolder = Directory.GetParent(path).FullName;
            var folders = Directory.GetDirectories(ParentFolder);
            for (int i = 0; i < folders.Length; i++)
            {
                if (folders[i].ToUpper() == path.ToUpper())
                {
                    DirectoryInfo di = new DirectoryInfo(folders[i]);
                    return di.Name;
                }
            }
            return "";
        }

        public static string GetCaseSensitiveFileName(string path)
        {
            string ParentFolder = Directory.GetParent(path).FullName;
            var files = Directory.GetFiles(ParentFolder);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].ToUpper() == path.ToUpper())
                {
                    FileInfo fi = new FileInfo(files[i]);
                    return fi.Name;
                }
            }
            return "";
        }
    }
}
