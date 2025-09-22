using System.Text.RegularExpressions;

namespace HBS.ImplementationAutomationConsole.Core.Extensions
{
    public static class StringExtension
    {
        public static bool CheckForValidStringResponse(this string str, bool hasWhiteSpace, bool hasSpecialCharacter, bool hasExceededLength, int length = 0)
        {
            if (hasWhiteSpace)
            {
                if (CheckIfHasWhiteSpace(str))
                {
                    return true;
                }
            }
            if (hasSpecialCharacter)
            {
                if (CheckIfHasSpecialCharacters(str))
                {
                    return true;
                }
            }
            if (hasExceededLength)
            {
                if (CheckIfExceededLength(str, length))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool CheckIfHasWhiteSpace(this string value)
        {
            if (value.Contains(" "))
                return true;

            return false;
        }

        private static bool CheckIfHasSpecialCharacters(this string value)
        {
            if (Regex.IsMatch(value, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
                return true;

            return false;
        }

        private static bool CheckIfExceededLength(this string value, int length)
        {
            if (value.Length > length)
                return true;

            return false;
        }
    }
}