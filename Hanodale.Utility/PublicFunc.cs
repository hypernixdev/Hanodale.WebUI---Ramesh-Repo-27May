using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Utility
{
    public class PublicFunc
    {
        public static string GenerateRandonCode(int length = 8)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
    }
}
