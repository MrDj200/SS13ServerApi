using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SS13ServerApi
{
    public static class Utils
    {
        public static bool IsValidAddress(string address) => !Regex.IsMatch(address, "(.*127.\\d.\\d.\\d)") && address != "localhost";
    }
}
