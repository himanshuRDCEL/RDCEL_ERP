using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RDCELERP.Common.Helper
{
    public class StringHelper
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }
        private static Random random = new Random();
        public static string RandomStrByLength(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string MaskVoucherCode(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length <= 2)
                return code; // Too short to mask

            return $"{code[0]}{new string('*', code.Length - 2)}{code[^1]}";
        }
    }
}