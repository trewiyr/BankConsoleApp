using System.Text.RegularExpressions;

namespace BankApp
{
    public static class Check
    {
        public static bool IsValid(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            string pattern = @"^[A-Za-z]{3,}([-A-Za-z\s]+)*$";
            return Regex.IsMatch(name, pattern);
        }
    }




}