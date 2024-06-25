using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SortingData
{
    /// <summary>
    /// Проверка данных
    /// </summary>
    public static class Validator
    {
        public static bool IsValid(ExtracteData dataObject)
        {
            if (string.IsNullOrEmpty(dataObject.Title) || string.IsNullOrEmpty(dataObject.Connect))
            {
                return false;
            }

            if (dataObject.Connect.StartsWith("File="))
            {
                string filePath = dataObject.Connect.Substring(5).Trim('"', ';');
                return IsValidFilePath(filePath);
            }
            else if (dataObject.Connect.StartsWith("Srvr="))
            {
                string connectString = dataObject.Connect.Substring(5).Trim('"', ';');
                string[] parts = connectString.Split(';');
                string hostPart = parts.FirstOrDefault(p => p.StartsWith("host=") || p.StartsWith("host"));
                string refPart = parts.FirstOrDefault(p => p.StartsWith("Ref="));

                return !string.IsNullOrEmpty(hostPart) && !string.IsNullOrEmpty(refPart);
            }

            return false;
        }

        private static bool IsValidFilePath(string path)
        {
            string pattern = @"^(?:[a-zA-Z]:|\\\\[a-zA-Z0-9_.$-]+\\[a-zA-Z0-9_.$-]+)\\(?:[a-zA-Z0-9(){}\[\]!@#%&+=._-]+\\)*[a-zA-Z0-9(){}\[\]!@#%&+=._-]*$";
            return Regex.IsMatch(path, pattern);
        }
    }
}
