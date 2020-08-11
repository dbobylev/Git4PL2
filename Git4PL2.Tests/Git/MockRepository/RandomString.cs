using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Tests.Git.MockRepository
{
    class RandomString
    {
        private static Random random = new Random();
        public const string charsKir = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮйцукенгшщзхъфывапролджэячсмитьбю";
        public const string charsLat = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public const string charsDig = "0123456789";

        public static string GetTxtRow()
        {
            return $"{GetTxt(8, charsLat)} := {GetTxt(3, charsDig)} -- !! {string.Join(" ", Enumerable.Range(0, 4).Select(x=> GetTxt(8, charsKir)))}";
        }

        public static string GetFileName()
        {
            return $"{GetTxt(10, charsLat)}.txt";
        }

        public static string GetTxt(int length, string chars)
        {
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
