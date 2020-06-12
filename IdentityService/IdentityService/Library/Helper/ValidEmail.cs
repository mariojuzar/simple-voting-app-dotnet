using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IdentityService.Library.Helper
{
    public class ValidEmail : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string strValue = value as string;
            if (strValue != null)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(strValue);
                if (match.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
