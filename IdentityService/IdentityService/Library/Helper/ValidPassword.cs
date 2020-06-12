using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IdentityService.Library.Helper
{
    public class ValidPassword : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string strValue = value as string;
            if (strValue != null)
            {
                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasMinimum8Chars = new Regex(@".{8,}");

                return hasNumber.IsMatch(strValue) && hasUpperChar.IsMatch(strValue)
                    && hasLowerChar.IsMatch(strValue) && hasMinimum8Chars.IsMatch(strValue);
            }
            return false;
        }
    }
}
